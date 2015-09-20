using System;

using Xamarin.Forms;
using Akavache;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace LunchPac
{
    public class App : Application
    {
        const string LunchPacUrl = "http://lunchpac.marathondata.com/";

        public App(PlatformModule module)
        {
            var bootStrap = new BootStrap(this);
            bootStrap.Run(module);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Task.Run(() =>
                {
                    LoginManager.LoggedinUser = null;
                    BlobCache.InMemory.InvalidateAll();
                    BlobCache.LocalMachine.InvalidateAll();
                });
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

