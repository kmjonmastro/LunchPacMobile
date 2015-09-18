using System;
using Autofac;
using Mobile.Core.Common.iOS.Utility;
using Mobile.Core;

namespace PestPacMobile.iOS
{
    public class iOSModule: PlatformModule
    {
        public override void RegisterPlatformServices(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<Refractored.Xam.Vibrate.Vibrate>();
        }
    }
}

