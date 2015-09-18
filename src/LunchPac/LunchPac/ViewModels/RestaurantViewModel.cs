using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LunchPac
{
    public class RestaurantViewModel: ViewModelBase
    {
        public string _MenuUrl;

        public string MenuUrl { get { return _MenuUrl; } set { SetRaiseIfPropertyChanged(ref _MenuUrl, value); } }

        public ObservableCollection<Order> _PreviousOrders;

        public ObservableCollection<Order> PreviousOrders { get { return _PreviousOrders; } set { SetRaiseIfPropertyChanged(ref _PreviousOrders, value); } }

        public Restaurant _Restaurant;

        public Restaurant Restaurant
        {
            get { return _Restaurant; }
            set
            {
                var rest = value;
                _Restaurant = rest;
                _MenuUrl = rest.Menu;

                Task.Run(async () =>
                    {
                        //TODO: go get the orders online;

                        //Dummy Data
                        Device.BeginInvokeOnMainThread(() =>
                            {
                                PreviousOrders = new ObservableCollection<Order>(){ new Order{ OrderItem = "Tomatoe Soup", RestaurantId = rest.RestaurantId } };
                            });
                    });
            }
        }

        readonly INavigator Navigator;

        public RestaurantViewModel(INavigator Navigator)
        {
            this.Navigator = Navigator;
        }

        public void PreviousOrderSelected(Order order)
        {
            if (order != null)
            {
                Navigator.PushAsync<OrderFormViewModel>((vm) =>
                    {
                    });
            }
        }

        public void MenuSelected()
        {
            
        }
    }
}

