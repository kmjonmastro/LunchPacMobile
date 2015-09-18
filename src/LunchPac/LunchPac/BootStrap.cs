using Autofac;
using Xamarin.Forms;
using Akavache;

namespace LunchPac
{
    public  class BootStrap : BootStrapper
    {
        Application _application;

        public BootStrap(Application app)
        {
            _application = app; 
        }

        protected override void ConfigureApplication(IContainer container)
        {
            //Init Cache
            BlobCache.ApplicationName = "lunchpac";

            // set main page
            var viewFactory = container.Resolve<IViewFactory>();
            var mainPage = viewFactory.Resolve<LoginViewModel>();
            var navigationPage = new NavigationPage(mainPage);
            _application.MainPage = navigationPage;
        }

        protected override void ConfigureCore(ContainerBuilder builder)
        {
            builder.RegisterType<LoginViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<LoginPage>().AsSelf().SingleInstance();

            builder.RegisterType<LandingPage>().AsSelf().SingleInstance();
            builder.RegisterType<LandingPageViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<OrderFormPage>().AsSelf().SingleInstance();
            builder.RegisterType<OrderFormViewModel>().AsSelf().SingleInstance();

            base.ConfigureCore(builder);
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<LoginViewModel, LoginPage>();
            viewFactory.Register<LandingPageViewModel, LandingPage>();
            viewFactory.Register<OrderFormViewModel, OrderFormPage>();
        }
    }
}

