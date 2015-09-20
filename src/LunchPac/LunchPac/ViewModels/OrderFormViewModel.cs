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

        bool SubmitButtonEnabled { get { return _submitButtonEnabled; } set { SetRaiseIfPropertyChanged(ref _submitButtonEnabled, value); } }

        string _submitButtontext;

        string SubmitButtonText { get { return _submitButtontext; } set { SetRaiseIfPropertyChanged(ref _submitButtontext, value); } }

        readonly INavigator Navigator;
        readonly DomainManager DomainManager;

        Order Order { get; set; }

        public OrderFormViewModel(INavigator navigator, DomainManager domainManager)
        {
            Title = "My Order";
            Navigator = navigator;
            DomainManager = domainManager;
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
                            });
                    }
                });
        }
    }
}


