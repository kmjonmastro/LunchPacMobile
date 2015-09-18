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

            Restaurants = new ObservableCollection<Restaurant>()
            {
                new Restaurant{ RestaurantName = "Taste Of Italy", RestaurantId = 1 },
                new Restaurant{ RestaurantName = "WAAB", RestaurantId = 2 }
            };

            Orders = new ObservableCollection<Order>()
            {
                new Order{ OrderItem = "Fried Chicken", RestaurantId = 1, OrderComments = "Fries" },
                new Order{ OrderItem = "Bean Soup", RestaurantId = 2, OrderComments = "Fruit Cup" }
            };
        }

    }
}


