using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Akavache;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive;
using System.Net.Http;
using ModernHttpClient;
using System.Net;
using Mobile.Core;
using Newtonsoft.Json;
using System.Linq;

namespace LunchPac
{
    public class DomainManager
    {
        public bool OrderingStatusOpen { get; private set; }

        public DomainManager()
        {
            OrderingStatusOpen = false;
        }

        async Task<List<Restaurant>> FetchRestaurants()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.RestaurantsUrl + "?date=" + WebUtility.UrlEncode(DateTime.UtcNow.ToISOString())))
                {
                    try
                    {
                        var res = await client.RequestAsync<List<Restaurant>>(req);
                        return res.Data;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to retrive list of restaurants", e);
                    }
                }
            }
        }

        public async Task<List<Restaurant>> GetOrFetchRestaurantsAsync()
        {
            try
            {
                return await BlobCache.InMemory.GetOrFetchObject < List<Restaurant>>(
                    Configuration.CacheKeys.Restaurants, 
                    async () => await FetchRestaurants().ConfigureAwait(false),
                    DateTimeOffset.UtcNow.AddDays(-1).Date
                );
            }
            catch (Exception e)
            {
                throw new Exception("Unable to download list of restaurants", e);
            }
        }

        public async Task<Order> GetCurrentOrder()
        {
            var history = await GetHistory();
            var today = DateTime.Today;
            foreach (var key in history.Keys)
            {
                foreach (var oder in history[key])
                {
                    var date = oder.AddDate.ToLocalTime().Date;
                    if (date == today)
                    {
                        return oder;
                    }
                }
            }
            return null;
        }

        public async Task<Dictionary<int, List<Order>>> GetHistory()
        {
            var history = new Dictionary<int, List<Order>>();

            try
            {
                history = await BlobCache.InMemory.GetObject<Dictionary<int, List<Order>>>(Configuration.CacheKeys.OrderHistory);
            }
            catch (KeyNotFoundException)
            {
            }

            return history;
        }

        public async Task<List<Order>> GetHistory(int RestaurantId)
        {
            return (await GetHistory().ConfigureAwait(false))[RestaurantId];
        }

        public async Task<OrderStatus> FetchOrderingStatus()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.OrderingStatus + "?date=" + WebUtility.UrlEncode(DateTime.UtcNow.ToISOString())))
                {
                    try
                    {
                        var resp = await client.RequestAsync<OrderStatus>(req);

                        OrderingStatusOpen = !resp.Data.Closed;

                        return resp.Data;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to retrieve odering status:" + e.FullMessage(), e);
                    }
                }
            }
        }

        public async Task<Dictionary<int, List<Order>>> FetchHistory(IEnumerable<Restaurant> rests)
        {
            var restDic = new Dictionary<int, List<Order>>();

            foreach (var res in rests)
            {
                var hist = await FetchHistoryAsync(res);
                restDic.Add(res.RestaurantId, hist);
            }

            await BlobCache.InMemory.InsertObject<Dictionary<int, List<Order>>>(Configuration.CacheKeys.OrderHistory, restDic);
            return restDic;
        }

        static async Task<List<Order>> FetchHistoryAsync(Restaurant rest)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.OrderhistoryUrl + "?userid=" + LoginManager.LoggedinUser.UserId + "&restaurantId=" + rest.RestaurantId))
                {
                    try
                    {
                        var res = await client.RequestAsync<List<Order>>(req);
                        return res.Data;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Unable to fetch history Error:" + e.FullMessage(), e);
                    }
                }
            }
        }


        public async Task DeleteOrder(Order order)
        {
            await FetchOrderingStatus();

            if (!OrderingStatusOpen)
            {
                throw new Exception("Lunch ordering is closed :(.\nPlease contact the responsible team directly.");
            }
//            #if DEBUG
//            throw new Exception("Not Available in Debug Mode");
//            #endif

            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Delete, Configuration.Routes.Order + "/?orderid=" + order.OrderId))
                {
                    try
                    {
                        await client.RequestAsync(req);
                      
                        var history = await GetHistory();
                        history[order.RestaurantId].RemoveAll(o => o.OrderId == order.OrderId);
                        await BlobCache.InMemory.InsertObject<Dictionary<int, List<Order>>>(Configuration.CacheKeys.OrderHistory, history);
                    }
                    catch (NetworkException e)
                    {
                        throw new Exception("You must be online.", e);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to delete order. Error:" + e.FullMessage(), e);
                    }
                }
            }
        }

        public async Task UpsertOrder(Order order)
        {
            if (string.IsNullOrEmpty(order.OrderItem) && string.IsNullOrEmpty(order.Soup) && string.IsNullOrEmpty(order.OrderComments))
            {
                throw new Exception("You must fill at least one field.");
            }
                
            await FetchOrderingStatus();

            if (!OrderingStatusOpen)
            {
                throw new Exception("Lunch ordering is closed :(.\nPlease contact the responsible team directly.");
            }

            order.AddDate = DateTime.UtcNow;
            order.UserId = LoginManager.LoggedinUser.UserId;

            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                var method = order.OrderId.HasValue ? HttpMethod.Put : HttpMethod.Post;
                using (var req = new HttpRequestMessage(method, Configuration.Routes.Order))
                {
                    try
                    {
                        var history = await GetHistory();
                        var json = JsonConvert.SerializeObject(order);
                        req.Content = new JsonContent(json);

                        if (order.OrderId.HasValue)
                        {
                            await client.RequestAsync(req);
                            history[order.RestaurantId].RemoveAll(o => o.OrderId == order.OrderId);
                        }
                        else
                        {
                            var res = await client.RequestAsync<int>(req);
                            order.OrderId = res.Data;
                        }

                        history[order.RestaurantId].Add(order);
                        var sorted = history[order.RestaurantId].OrderByDescending(o => o.AddDate).ToList();
                        history[order.RestaurantId] = sorted;
                        await BlobCache.InMemory.InsertObject<Dictionary<int, List<Order>>>(Configuration.CacheKeys.OrderHistory, history);
                    }
                    catch (NetworkException e)
                    {
                        throw new Exception("You must be online.", e);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Unable to create order. Error:" + e.FullMessage(), e);
                    }
                }
            }
        }
    }
}

