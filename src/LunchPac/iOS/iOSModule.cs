using Autofac;

namespace LunchPac.iOS
{
    public class iOSModule: PlatformModule
    {
        public override void RegisterPlatformServices(ContainerBuilder builder)
        {
//            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
//            builder.RegisterType<Refractored.Xam.Vibrate.Vibrate>();
        }
    }
}

