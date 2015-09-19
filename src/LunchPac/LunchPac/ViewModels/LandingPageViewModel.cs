using System;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LunchPac
{
    public class LandingPageViewModel :  ViewModelBase
    {
        readonly INavigator Navigator;
        readonly DomainManager DomainManager;

        ObservableCollection<Restaurant> _Restaurants;

        public ObservableCollection<Restaurant> Restaurants { get { return _Restaurants; } set { SetRaiseIfPropertyChanged(ref _Restaurants, value); } }

        ObservableCollection<Order> _Orders;

        public ObservableCollection<Order> Orders { get { return _Orders; } set { SetRaiseIfPropertyChanged(ref _Orders, value); } }

        public LandingPageViewModel(INavigator Navigator, DomainManager DomainManager)
        {
            this.DomainManager = DomainManager;
            this.Navigator = Navigator;
        }

        public void OnAppearing()
        {
            DomainManager.GetOrFetchRestaurantsAsync().ContinueWith((t) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            if (t.Result != null)
                                Restaurants = new ObservableCollection<Restaurant>(t.Result);
                            #if DEBUG
                            if (t.Result == null)
                            {
                                const string json = "[{\"restaurantId\":22,\"restaurantName\":\"Muscle Maker Grill\",\"monday\":false,\"tuesday\":false,\"wednesday\":true,\"thursday\":false,\"friday\":false,\"menu\":\"MuscleMakerGrill.asp\"},{\"restaurantId\":11,\"restaurantName\":\"Surf Taco\",\"monday\":false,\"tuesday\":false,\"wednesday\":true,\"thursday\":false,\"friday\":false,\"menu\":\"SurfTaco.asp\"}]";
                                Restaurants = new ObservableCollection<Restaurant>(JsonConvert.DeserializeObject<List<Restaurant>>(json));
                            }
                            #endif 
                        });
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void HandleRestaurantSelected(Restaurant rest)
        {
            if (rest != null)
                Navigator.PushAsync<RestaurantViewModel>((vm) => vm.SetRestaurant(rest));
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


