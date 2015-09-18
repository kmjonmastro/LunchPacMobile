using System;
using System.Collections.ObjectModel;

namespace LunchPac
{
    public class RestaurantViewModel: ViewModelBase
    {
        public string _MenuUrl;

        public string MenuUrl { get { return _MenuUrl; } set { SetRaiseIfPropertyChanged(ref _MenuUrl, value); } }

        public ObservableCollection<Order> _PreviousOrders;

        public ObservableCollection<Order> PreviousOrders { get { return _PreviousOrders; } set { SetRaiseIfPropertyChanged(ref _PreviousOrders, value); } }

        public Restaurant Restaurant { get; set; }

        readonly INavigator Navigator;

        public RestaurantViewModel(INavigator Navigator)
        {
            this.Navigator = Navigator;
        }
    }
}

