using Autofac;
using Xamarin.Forms;
using Akavache;
using System.Threading.Tasks;

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
            BlobCache.ApplicationName = Configuration.DbName;

            // set main page
            var viewFactory = container.Resolve<IViewFactory>();
            Page mainPage; 
            mainPage = viewFactory.Resolve<LandingPageViewModel>();
            var navigationPage = new NavigationPage(mainPage);
            _application.MainPage = navigationPage;
        }

        protected override void ConfigureCore(ContainerBuilder builder)
        {
            builder.RegisterType<LoginManager>().AsSelf().SingleInstance();
            builder.RegisterType<DomainManager>().AsSelf().SingleInstance();

            builder.RegisterType<LoginViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<LoginPage>().AsSelf().SingleInstance();

            builder.RegisterType<LandingPage>().AsSelf().SingleInstance();
            builder.RegisterType<LandingPageViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<OrderFormPage>().AsSelf().SingleInstance();
            builder.RegisterType<OrderFormViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<RestaurantPage>().AsSelf().SingleInstance();
            builder.RegisterType<RestaurantViewModel>().AsSelf().SingleInstance();

            base.ConfigureCore(builder);
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<LoginViewModel, LoginPage>();
            viewFactory.Register<LandingPageViewModel, LandingPage>();
            viewFactory.Register<OrderFormViewModel, OrderFormPage>();
            viewFactory.Register<RestaurantViewModel, RestaurantPage>();

        }
    }
}

