using Autofac;


namespace LunchPac.Droid
{
    public class AndroidModule : PlatformModule
    {
        public override void RegisterPlatformServices(ContainerBuilder builder)
        {
//            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
//            builder.RegisterType<Refractored.Xam.Vibrate.Vibrate>();
        }
    }
}

