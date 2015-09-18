using System;
using System.Net.Http;
using ModernHttpClient;
using Mobile.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;
using System.Reactive;
using System.Collections.Generic;

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

        public async Task DeleteToken()
        {
            try
            {
                await BlobCache.Secure.EraseLogin();
            }
            catch (KeyNotFoundException e)
            {
            }
        }

        public Task<bool> LoginFromSession()
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                BlobCache.Secure.GetLoginAsync().Subscribe((token) =>
                    {
                        if (token == null)
                        {
                            tcs.SetResult(false);
                        }

                        Task.Run(async () =>
                            {
                                try
                                {
                                    var res = await LoginAsync(token.UserName, token.Password, true);
                                    tcs.SetResult(true);
                                }
                                catch (Exception)
                                {
                                    tcs.SetResult(false);
                                }
                            });
                    });

                return tcs.Task;
            }
            catch (Exception e)
            {
                tcs.SetResult(false);
            }

            return tcs.Task;
        }

        public async Task<bool> LoginAsync(string uName, string pwd, bool fromCache = false)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, Configuration.LoginUrl))
                {
                    request.Content = new JsonContent(JsonConvert.SerializeObject(new LoginCredentials(uName, pwd)));

                    var resp = await client.RequestAsync<LoginResponse>(request);

                    if (resp.Data == null || !resp.Data.Success)
                    {
                        throw new Exception("Login Failed" + ((resp.Data != null && !string.IsNullOrEmpty(resp.Data.ErrorMessage)) ? "\n Error: " + resp.Data.ErrorMessage : string.Empty));
                    }

                    await BlobCache.Secure.SaveLogin(uName, pwd, absoluteExpiration: DateTime.Today.AddHours(8));

                    return true;
                }
            }
        }
    }
}

