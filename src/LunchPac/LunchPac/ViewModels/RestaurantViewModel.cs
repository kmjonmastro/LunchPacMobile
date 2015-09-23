using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace LunchPac
{
    public class RestaurantViewModel: ViewModelBase
    {
        //Xamarin doesn't have conditinal binding, so we need another layer of indirection between the Order Model and the View
        //        public class OrderModel
        //        {
        //            public bool CurrentOrder { get; set; }
        //            public Color Background { get; set; }
        //            public Order Order { get; set; }
        //
        //            public OrderModel(Order order, bool currentOrder)
        //            {
        //                Order = order;
        //                CurrentOrder = currentOrder;
        //                Background = currentOrder ? Color.FromHex("#81C134") : Color.White;
        //            }
        //        }
        //
        public class OrderModel : Order
        {
            public bool IsCurrentOrder { get; set; }
        }

        public string _MenuUrl;

        public string MenuUrl { get { return _MenuUrl; } set { SetRaiseIfPropertyChanged(ref _MenuUrl, value); } }

        public string _OrderButtonText;

        public string OrderButtonText { get { return _OrderButtonText; } set { SetRaiseIfPropertyChanged(ref _OrderButtonText, value); } }

        public bool _OrderButtonEnabled;

        public bool OrderButtonEnabled { get { return _OrderButtonEnabled; } set { SetRaiseIfPropertyChanged(ref _OrderButtonEnabled, value); } }

        public Color _OrderButtonColor;

        public Color OrderButtonColor { get { return _OrderButtonColor; } set { SetRaiseIfPropertyChanged(ref _OrderButtonColor, value); } }

        public ObservableCollection<Order> _PreviousOrders;

        public ObservableCollection<Order> PreviousOrders { get { return _PreviousOrders; } set { SetRaiseIfPropertyChanged(ref _PreviousOrders, value); } }

        Restaurant Restaurant
        {
            get;
            set;
        }

        Order CurrentOrder { get; set; }

        Order ExistingOrder { get; set; }

        readonly INavigator Navigator;
        readonly  DomainManager DomainManager;

        public RestaurantViewModel(INavigator cavigator, DomainManager domainManager)
        {
            Navigator = cavigator;
            DomainManager = domainManager;
        }

        public void SetRestaurant(Restaurant rest)
        {
            const string UpdateOrderTxt = "Edit my order";
            const string NewOrderTxt = "I am ready!";
            const string NewOrderButtonColor = "#81C134";
            const string UpdateOrderColor = "#ED8413";

            Restaurant = rest;
            MenuUrl = Restaurant.GetMenuUrl();
            Title = Restaurant.RestaurantName;
            OrderButtonText = NewOrderTxt;
            OrderButtonColor = Color.FromHex(NewOrderButtonColor);
            OrderButtonEnabled = false;
            CurrentOrder = null;
            ExistingOrder = null;
            //fetch the orders
            Task.Run(async () =>
                {
                    ExistingOrder = await DomainManager.GetCurrentOrder();
                    var filtered = await DomainManager.GetHistory(Restaurant.RestaurantId);
                    var currentOrder = filtered.FirstOrDefault(o => o.AddDate.ToLocalTime().Date == DateTime.Today);
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            var hasOrderForToday = currentOrder != null;
                            if (hasOrderForToday)
                            {
                                OrderButtonText = UpdateOrderTxt;
                                OrderButtonColor = Color.FromHex(UpdateOrderColor);
                                CurrentOrder = currentOrder;
                                currentOrder.IsCurrentOrder = true;
                            }
                            else
                            {
                                OrderButtonText = NewOrderTxt;
                                OrderButtonColor = Color.FromHex(NewOrderButtonColor);
                                CurrentOrder = new Order() { RestaurantId = this.Restaurant.RestaurantId };
                            }
//                            //Hack to allow current order to be highlighted
//                            var models = filtered.Select((order) =>
//                                {
//                                
//                                    OrderModel orderModel = new OrderModel(order, order.OrderId == ExistingOrder?.OrderId);
//                                    return orderModel;
//                                });
                            PreviousOrders = new ObservableCollection<Order>(filtered);
                            OrderButtonEnabled = true;
                        });
                });
        }

        public void Refresh()
        {
            if (Restaurant != null)
                SetRestaurant(Restaurant);
        }

        public void OrderButtonClicked()
        {
            if (ExistingOrder != null && ExistingOrder.RestaurantId != Restaurant.RestaurantId)
                Task.Run(async () =>
                    {
                        var takemethere = await Application.Current.MainPage.DisplayAlert("Oops!", "You already have an order a different restaurant. what would you like to do ?", "Take me there!", "Cancel");
                        if (takemethere)
                        {
                            var restaurants = await DomainManager.GetOrFetchRestaurantsAsync();
                            var rest = restaurants.FirstOrDefault(r => r.RestaurantId == ExistingOrder.RestaurantId);
                            Device.BeginInvokeOnMainThread(() =>
                                {
                                    if (rest != null)
                                        SetRestaurant(rest);
                                });
                        }
                    });
            else
                Navigator.PushAsync<OrderFormViewModel>((vm) =>
                vm.SetOrder(CurrentOrder));
        }

        public void HandleHistoryItemClicked(Order order)
        {
            if (order != null)
            {
                if (ExistingOrder != null)
                    order.OrderId = ExistingOrder.OrderId;
                else
                    order.OrderId = null;
                Navigator.PushAsync<OrderFormViewModel>((vm) => vm.SetOrder(order));
            }
        }
    }
}

