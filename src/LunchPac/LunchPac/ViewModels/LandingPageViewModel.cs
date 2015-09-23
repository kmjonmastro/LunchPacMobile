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

        public bool _OrderButtonVisible;

        public bool OrderButtonVisible { get { return _OrderButtonVisible; } set { SetRaiseIfPropertyChanged(ref _OrderButtonVisible, value); } }

        private Order CurrentOrder;

        public LandingPageViewModel(INavigator Navigator, DomainManager DomainManager)
        {
            this.DomainManager = DomainManager;
            this.Navigator = Navigator;
            Refresh();
        }

        public async void Refresh()
        {
            CurrentOrder = await DomainManager.GetCurrentOrder();
            OrderButtonVisible = CurrentOrder != null;

            try
            {
                var list = await DomainManager.GetOrFetchRestaurantsAsync().ConfigureAwait(false);
                Device.BeginInvokeOnMainThread(() =>
                    {
                        if (LoginManager.LoggedinUser == null)
                        {
                            Navigator.PushModalAsync<LoginViewModel>();
                            return;
                        }
                        if (list != null)
                            Restaurants = new ObservableCollection<Restaurant>(list);
                        #if DEBUG
                        if (list == null)
                        {
                            const string json = "[{\"restaurantId\":22,\"restaurantName\":\"Muscle Maker Grill\",\"monday\":false,\"tuesday\":false,\"wednesday\":true,\"thursday\":false,\"friday\":false,\"menu\":\"MuscleMakerGrill.asp\"},{\"restaurantId\":11,\"restaurantName\":\"Surf Taco\",\"monday\":false,\"tuesday\":false,\"wednesday\":true,\"thursday\":false,\"friday\":false,\"menu\":\"SurfTaco.asp\"}]";
                            Restaurants = new ObservableCollection<Restaurant>(JsonConvert.DeserializeObject<List<Restaurant>>(json));
                        }
                        #endif 
                    });
            }
            catch (Exception e)
            {
            }
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

        public void HandleOrderButtonClicked()
        {
            if (CurrentOrder != null)
                Navigator.PushAsync<OrderFormViewModel>((vm) => vm.SetOrder(CurrentOrder));
        }
    }
}


