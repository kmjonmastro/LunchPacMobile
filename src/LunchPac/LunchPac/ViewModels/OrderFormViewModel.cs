using System;

using Xamarin.Forms;

namespace LunchPac
{
    public class OrderFormViewModel : ViewModelBase
    {
        Restaurant _Restaurant;

        public Restaurant Restaurant { get { return _Restaurant; } set { SetRaiseIfPropertyChanged(ref _Restaurant, value); } }

        string _OrderItem;

        public string OrderItem { get { return _OrderItem; } set { SetRaiseIfPropertyChanged(ref _OrderItem, value); } }

        string _Soup;

        public string Soup { get { return _Soup; } set { SetRaiseIfPropertyChanged(ref _Soup, value); } }

        string _SoupSize;

        public string SoupSize { get { return _SoupSize; } set { SetRaiseIfPropertyChanged(ref _SoupSize, value); } }

        string _OrderComments;

        public string OrderComments { get { return _OrderComments; } set { SetRaiseIfPropertyChanged(ref _OrderComments, value); } }

        readonly INavigator Navigator;

        public OrderFormViewModel(INavigator navigator)
        {
            Navigator = navigator;
        }
    }
}


