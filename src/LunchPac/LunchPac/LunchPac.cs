using System;

using Xamarin.Forms;

namespace LunchPac
{
    public class App : Application
    {
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
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

