using System;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace LunchPac
{
    public class OrderFormViewModel : ViewModelBase
    {
        Restaurant _Restaurant;

        public Restaurant Restaurant { get { return _Restaurant; } set { _Restaurant = value; } }

        string _OrderItem;

        public string OrderItem { get { return _OrderItem; } set { SetRaiseIfPropertyChanged(ref _OrderItem, value); } }

        string _Soup;

        public string Soup { get { return _Soup; } set { SetRaiseIfPropertyChanged(ref _Soup, value); } }

        string _SoupSize;

        public string SoupSize { get { return _SoupSize; } set { SetRaiseIfPropertyChanged(ref _SoupSize, value); } }

        string _OrderComments;

        public string OrderComments { get { return _OrderComments; } set { SetRaiseIfPropertyChanged(ref _OrderComments, value); } }

        bool _submitButtonEnabled = true;

        public  bool SubmitButtonEnabled { get { return _submitButtonEnabled; } set { SetRaiseIfPropertyChanged(ref _submitButtonEnabled, value); } }

        string _submitButtontext;

        public string SubmitButtonText { get { return _submitButtontext; } set { SetRaiseIfPropertyChanged(ref _submitButtontext, value); } }

        bool _deleteButtonEnabled = false;

        public bool DeleteButtonEnabled { get { return _deleteButtonEnabled; } set { SetRaiseIfPropertyChanged(ref _deleteButtonEnabled, value); } }

        bool _deleteButtonVisible = false;

        public bool DeleteButtonVisible { get { return _deleteButtonVisible; } set { SetRaiseIfPropertyChanged(ref _deleteButtonVisible, value); } }

        readonly INavigator Navigator;
        readonly DomainManager DomainManager;
        readonly RestaurantViewModel RestaurantViewModel;

        Order Order { get; set; }

        public OrderFormViewModel(INavigator navigator, DomainManager domainManager, RestaurantViewModel RestaurantViewModel)
        {
            Title = "My Order";
            Navigator = navigator;
            DomainManager = domainManager;
            this.RestaurantViewModel = RestaurantViewModel;
            SubmitButtonEnabled = DomainManager.OrderingStatusOpen;
        }

        public void SetOrder(Order order)
        {
            Order = order;

            ResetFields();
        }

        void ResetFields()
        {
            Soup = Order.Soup ?? string.Empty;
            OrderItem = Order.OrderItem ?? string.Empty;
            OrderComments = Order.OrderComments ?? string.Empty;
            SubmitButtonEnabled = DomainManager.OrderingStatusOpen;
            SubmitButtonText = Order.OrderId.HasValue ? "Update" : "Create";
            DeleteButtonEnabled = Order.OrderId.HasValue;
            DeleteButtonVisible = Order.OrderId.HasValue;
        }

        public void CancelOrder()
        {
            SubmitButtonEnabled = false;
            Order.Soup = Soup;
            Order.OrderComments = OrderComments;
            Order.OrderItem = OrderItem;
            DeleteButtonEnabled = false;

            Task.Run(async () =>
                {
                    Exception ex = null;
                    try
                    {
                        await DomainManager.DeleteOrder(Order);
                    }
                    catch (Exception e)
                    {
                        ex = e;
                    }
                    finally
                    {
                        Device.BeginInvokeOnMainThread(() =>
                            {
                                ResetFields();
                                Application.Current.MainPage.DisplayAlert("Oh Snap :(", ex.Message, "OK");
                                if (ex != null)
                                {
                                    RestaurantViewModel.Refresh();
                                } 
                            });
                    }
                });
        }

        public void ValidateAndSubmit()
        {
            SubmitButtonEnabled = false;
            Order.Soup = Soup;
            Order.OrderComments = OrderComments;
            Order.OrderItem = OrderItem;

            Task.Run(async () =>
                {
                    Exception ex = null;
                    try
                    {
                        await DomainManager.UpsertOrder(Order);
                    }
                    catch (Exception e)
                    {
                        ex = e;
                    }
                    finally
                    {
                        Device.BeginInvokeOnMainThread(() =>
                            {
                                ResetFields();
                                var title = ex != null ? "Oh Snap :(" : "Yesss!";
                                var message = ex != null ? ex.Message : "Your order has been submited!";
                                Application.Current.MainPage.DisplayAlert(title, message, "OK");
                                if (ex != null)
                                {
                                    RestaurantViewModel.Refresh();
                                } 
                            });
                    }
                });
        }
    }
}


