﻿using System;
using Autofac.Core;
using Autofac;

namespace LunchPac
{
    public abstract class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            RegisterPlatformServices(builder);
        }

        public abstract void RegisterPlatformServices(ContainerBuilder builder);
    }
}

