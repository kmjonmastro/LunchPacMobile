﻿using System;
using System.Collections.Generic;
using Autofac;
using Xamarin.Forms;

namespace LunchPac
{
    public class ViewFactory : IViewFactory
    {
        readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        readonly IComponentContext _componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void Register<TViewModel, TView>() 
            where TViewModel : class, IViewModel 
            where TView : Page
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve<TViewModel>(out viewModel, setStateAction);
        }

        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel
        {
            viewModel = _componentContext.Resolve<TViewModel>();

            var viewType = _map[typeof(TViewModel)];
            var view = _componentContext.Resolve(viewType) as Page;

            if (setStateAction != null)
                viewModel.SetState(setStateAction);

            view.BindingContext = viewModel;
            return view;
        }

        public Page Resolve<TViewModel>(TViewModel viewModel) 
            where TViewModel : class, IViewModel
        {
            var viewType = _map[typeof(TViewModel)];
            var view = _componentContext.Resolve(viewType) as Page;
            view.BindingContext = viewModel;
            return view;
        }
    }
}

