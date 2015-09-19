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

namespace LunchPac
{
    public class DomainManager
    {
        public DomainManager()
        {
        }

        async Task<List<Restaurant>> FetchRestaurants()
        {
            var date = WebUtility.UrlEncode(DateTime.UtcNow.ToString("MM-dd-yyyy"));
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, Configuration.Routes.RestaurantsUrl + "?" + date))
                {
                    try
                    {
                        var res = await client.RequestAsync<List<Restaurant>>(req);
                        return res.Data;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.FullMessage());
                        throw;
                    }
                }
            }
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return await BlobCache.LocalMachine.GetOrFetchObject < List<Restaurant>>(
                Configuration.CacheKeys.Restaurants, 
                async () => await FetchRestaurants().ConfigureAwait(false),
                DateTimeOffset.Now.AddMinutes(10)
            );
        }
    }
}

