using Autofac;
using Mobile.Core.Common.Droid.Utility;
using Mobile.Core;

namespace PestPacMobile.Droid
{
    public class AndroidModule : PlatformModule
    {
        public override void RegisterPlatformServices(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<Refractored.Xam.Vibrate.Vibrate>();
        }
    }
}

