using System;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace LunchPac
{
    public class LandingPageViewModel :  ViewModelBase
    {
        readonly INavigator Navigator;

        ObservableCollection<Restaurant> _Restaurants;

        public ObservableCollection<Restaurant> Restaurants { get { return _Restaurants; } set { SetRaiseIfPropertyChanged(ref _Restaurants, value); } }

        ObservableCollection<Order> _Orders;

        public ObservableCollection<Order> Orders { get { return _Orders; } set { SetRaiseIfPropertyChanged(ref _Orders, value); } }

        public LandingPageViewModel(INavigator Navigator)
        {
            this.Navigator = Navigator;
        }

        public new void SetState<T>(Action<T> action) where T : class, IViewModel
        {
            action(this as T);
        }

        public void HandleRestaurantSelected(Restaurant rest)
        {
            if (rest != null)
                Navigator.PushAsync<RestaurantViewModel>((vm) => vm.Restaurant = rest);
        }

        public void HandleOrderSelected(Order order)
        {
            if (order != null)
                Navigator.PushAsync<OrderFormViewModel>((vm) =>
                    {
                    });
        }
    }
}


