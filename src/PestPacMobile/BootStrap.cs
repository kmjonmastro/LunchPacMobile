using Autofac;
using PestPac.Mobile.Core;
using System.Collections.Generic;
using Autofac.Core;
using Xamarin.Forms;
using Akavache;

namespace PestPacMobile
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
            //Set Cache
            BlobCache.ApplicationName = "workwave";

            // set main page
            var viewFactory = container.Resolve<IViewFactory>();
            var mainPage = viewFactory.Resolve<LoginViewModel>();
            var navigationPage = new NavigationPage(mainPage);
            _application.MainPage = navigationPage;
        }

        protected override void ConfigureCore(ContainerBuilder builder)
        {
            builder.RegisterType<LoginProxy>().AsSelf().SingleInstance();
            builder.RegisterType<AppointmentProxy>().AsSelf().SingleInstance();

            builder.RegisterType<LoginManager>().AsSelf().SingleInstance();

            builder.RegisterType<LoginViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<LoginPage>().AsSelf().SingleInstance();
            builder.RegisterType<ApptListPage>().AsSelf().SingleInstance();
            builder.RegisterType<ApptListViewModel>().AsSelf().SingleInstance();

            base.ConfigureCore(builder);
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<LoginViewModel, LoginPage>();
            viewFactory.Register<ApptListViewModel, ApptListPage>();
        }
    }
}

