using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace LunchPac
{
    public class RestaurantViewModel: ViewModelBase
    {
        public string _MenuUrl;

        public string MenuUrl { get { return _MenuUrl; } set { SetRaiseIfPropertyChanged(ref _MenuUrl, value); } }

        public bool _MenuButtonVisible;

        public bool MenuButtonVisible { get { return _MenuButtonVisible; } set { SetRaiseIfPropertyChanged(ref _MenuButtonVisible, value); } }

        public ObservableCollection<Order> _PreviousOrders;

        public ObservableCollection<Order> PreviousOrders { get { return _PreviousOrders; } set { SetRaiseIfPropertyChanged(ref _PreviousOrders, value); } }

        Restaurant Restaurant
        {
            get;
            set;
        }

        readonly INavigator Navigator;
        readonly  DomainManager DomainManager;

        public RestaurantViewModel(INavigator cavigator, DomainManager domainManager)
        {
            Navigator = cavigator;
            DomainManager = domainManager;
        }

        public void SetRestaurant(Restaurant rest)
        {
            Restaurant = rest;
            MenuUrl = Restaurant.GetMenuUrl();
            MenuButtonVisible = !string.IsNullOrEmpty(Restaurant.Menu);

            //fetch the orders
            Task.Run(async () =>
                {
                    var orders = await DomainManager.GetHistory();
                    var filtered = orders.Where(o => o.RestaurantId == Restaurant.RestaurantId);
                    PreviousOrders = new ObservableCollection<Order>(filtered);
                });
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

        public void NewOrderSelected()
        {
            Navigator.PushAsync<OrderFormViewModel>((vm) =>
                {
                });
        }

        public void MenuSelected()
        {
            Device.OpenUri(new Uri(Restaurant.GetMenuUrl()));
        }
    }
}

