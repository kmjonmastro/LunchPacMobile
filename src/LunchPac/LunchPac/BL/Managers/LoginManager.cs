using System;
using System.Net.Http;
using ModernHttpClient;
using Mobile.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Akavache;
using System.Collections.Generic;
using System.Reactive.Linq;
using Xamarin.Forms;

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
            catch (Exception)
            {
            }
        }

        public async Task<LoginInfo> GetLoginInfoAsync()
        {
            bool errored = false;
            var loginInfo = new LoginInfo("", "");
            try
            {
                //TODO: Firgure out why this does not work or errors out
                loginInfo = await BlobCache.Secure.GetLoginAsync()
                    .Catch((Func<KeyNotFoundException, IObservable<LoginInfo>>)((e) =>
                        Observable.Return(new LoginInfo(string.Empty, string.Empty))));
            }
            catch (KeyNotFoundException)
            {
                errored = true;
            }

            if (errored)
            {
                await DeleteToken();
            }

            return loginInfo;
        }

        public async Task<bool> LoginAsync(string uName, string pwd, bool fromCache = false)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, Configuration.Routes.LoginUrl))
                {
                    request.Content = new JsonContent(JsonConvert.SerializeObject(new LoginCredentials(uName, pwd)));

                    try
                    {
                        var resp = await client.RequestAsync<LoginResponse>(request);
                        if (resp.Data == null || !resp.Data.Success)
                        {
                            throw new Exception("Login Failed" + ((resp.Data != null && !string.IsNullOrEmpty(resp.Data.ErrorMessage)) ? "\n Error: " + resp.Data.ErrorMessage : string.Empty));
                        }
                        LoggedinUser = resp.Data.User;
                        await BlobCache.Secure.SaveLogin(uName, pwd);
                    }
                    catch (NetworkException n)
                    {
                        throw new Exception("Login server is unreachable", n);
                    }

                    return true;
                }
            }
        }
    }
}

