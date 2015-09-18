using Autofac;

namespace LunchPac
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // service registration
            builder.RegisterType<ViewFactory>()
                .As<IViewFactory>()
                .SingleInstance();

            builder.RegisterType<Navigator>()
                .As<INavigator>()
                .SingleInstance();

            // navigation registration
            builder.Register<Xamarin.Forms.INavigation>(context => 
                Xamarin.Forms.Application.Current.MainPage.Navigation
            ).SingleInstance();
        }
    }
}

