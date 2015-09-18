using System;
using Xamarin.Forms;
using Akavache;
using Autofac;
using Mobile.Core;
using PestPac.Mobile.Core;
using Autofac.Core;

namespace PestPacMobile
{
    public class App : Application
    {
        public ILogger Logger { get; set; }

        public LoginProxy LoginProxy { get; set; }

        public App(PlatformModule platformModule)
        {
            var bootStrap = new BootStrap(this);
            bootStrap.Run(platformModule);
        }

        protected override void OnStart()
        {
            // Handle when your app Starts
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

