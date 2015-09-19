using System;
using System.Net.Http;
using ModernHttpClient;
using Mobile.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Akavache;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace LunchPac
{
    public class LoginManager
    {
        public class LoginCredentials
        {
            public LoginCredentials(string uName, string pwd)
            {
                Username = uName;
                Password = pwd;
            }

            public string Username { get; set; }

            public string Password { get; set; }
        }


        public class LoginResponse
        {
            public bool Success { get; set; }

            public string ErrorMessage { get; set; }

            public User User { get; set; }
        }

        public static User LoggedinUser { get; private set; }

        public async Task DeleteToken()
        {
            try
            {
                await BlobCache.Secure.EraseLogin();
            }
            catch (KeyNotFoundException)
            {
            }
        }

        public async Task<LoginInfo> GetLoginInfoAsync()
        {
            var user = await BlobCache.Secure.GetLoginAsync();
            return user;
        }

        public async Task<bool> LoginFromSession()
        {
            var tcs = new TaskCompletionSource<bool>();

            var user = await GetLoginInfoAsync();
            return true;
        }

        public async Task<bool> LoginAsync(string uName, string pwd, bool fromCache = false)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, Configuration.Routes.LoginUrl))
                {
                    request.Content = new JsonContent(JsonConvert.SerializeObject(new LoginCredentials(uName, pwd)));

                    var resp = await client.RequestAsync<LoginResponse>(request);

                    if (resp.Data == null || !resp.Data.Success)
                    {
                        throw new Exception("Login Failed" + ((resp.Data != null && !string.IsNullOrEmpty(resp.Data.ErrorMessage)) ? "\n Error: " + resp.Data.ErrorMessage : string.Empty));
                    }

                    LoggedinUser = resp.Data.User;
                    await BlobCache.Secure.SaveLogin(uName, pwd, absoluteExpiration: DateTime.Today.AddHours(8));

                    return true;
                }
            }
        }
    }
}

