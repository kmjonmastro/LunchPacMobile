using System;
using Autofac;

namespace PestPacMobile
{
    public abstract class BootStrapper
    {
        public void Run(PlatformModule module = null)
        {
            var builder = new ContainerBuilder();

            ConfigureCore(builder);
            ConfigurePlatform(builder, module);

            var container = builder.Build();
            var viewFactory = container.Resolve<IViewFactory>();
            RegisterViews(viewFactory);

            ConfigureApplication(container);
        }

        protected void ConfigurePlatform(ContainerBuilder builder, PlatformModule module)
        {
            if (module != null)
                builder.RegisterModule(module);
        }

        protected virtual void ConfigureCore(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
        }

        protected abstract void ConfigureApplication(IContainer container);

        protected abstract void RegisterViews(IViewFactory viewFactory);

    }
}

