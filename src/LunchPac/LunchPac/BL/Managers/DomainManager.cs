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
        public bool OrderingStatus = false;

        public DomainManager()
        {
        }

        async Task<List<Restaurant>> FetchRestaurants()
        {
            var date = WebUtility.UrlEncode(DateTime.UtcNow.ToString("MM-dd-yyyy"));
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.RestaurantsUrl + "?date=" + date))
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
                return await BlobCache.LocalMachine.GetOrFetchObject < List<Restaurant>>(
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

        public async Task<Order> CurrentOrder()
        {
            var orders = await GetHistory();
            return orders.FirstOrDefault(o => o.AddDate.ToLocalTime().Date == DateTime.Today.Date);
        }

        //        public async Task UpsertOrderAsync(Order order)
        //        {
        //            using (var client = new HttpClient(new NativeMessageHandler()))
        //            {
        //                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.BaseUrl + "?user=" + LoginManager.LoggedinUser.UserId))
        //                {
        //                    try
        //                    {
        //                        var res = await client.RequestAsync<List<Order>>(req);
        //                        BlobCache.InMemory.InsertObject<List<Order>>(Configuration.CacheKeys.OrderHistory, res.Data);
        //                        return res.Data;
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        throw new Exception("Unable to fetch history Error:" + e.FullMessage(), e);
        //                    }
        //                }
        //            }
        //
        //            await BlobCache.InMemory.InsertObject(Configuration.CacheKeys.OrderHistory, orders);
        //        }

        public async Task<List<Order>> GetHistory()
        {
            var history = new List<Order>();

            try
            {
                history = await BlobCache.InMemory.GetObject<List<Order>>(Configuration.CacheKeys.OrderHistory);
            }
            catch (KeyNotFoundException)
            {
            }

            return history;
        }

        public async Task FetchOrderingStatus()
        {
            var date = WebUtility.UrlEncode(DateTime.UtcNow.ToString("s"));

            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.OrderingStatus + "?date=" + date))
                {
                    try
                    {
                        var resp = await client.RequestAsync<OrderStatus>(req);
                        OrderingStatus = resp.Data.Closed;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to retrieve odering status:" + e.FullMessage(), e);
                    }
                }
            }
        }

        public async Task<List<Order>> FetchHistory()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.OrderhistoryUrl + "?userid=" + LoginManager.LoggedinUser.UserId))
                {
                    try
                    {
                        var res = await client.RequestAsync<List<Order>>(req);
                        BlobCache.InMemory.InsertObject<List<Order>>(Configuration.CacheKeys.OrderHistory, res.Data);
                        return res.Data;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Unable to fetch history Error:" + e.FullMessage(), e);
                    }
                }
            }
        }
    }
}

