using Xamarin.Forms;
using System;

namespace LunchPac
{
    public partial class RestaurantPage : ContentPage
    {
        public RestaurantViewModel RestaurantViewModel { get; set; }

        public RestaurantPage(RestaurantViewModel restVm)
        {
            InitializeComponent();
            BindingContext = RestaurantViewModel = restVm;
        }

        void OrderSelected(object sender, EventArgs e)
        {
            var item = ((ListView)sender).SelectedItem as Order;
            ((ListView)sender).SelectedItem = null;
            RestaurantViewModel.PreviousOrderSelected(item);
        }

        void OnNewOrderClicked(object sender, EventArgs e)
        {
            RestaurantViewModel.NewOrderSelected();
        }

        void OnMenuClicked(object sender, EventArgs e)
        {
            RestaurantViewModel.MenuSelected();
        }
    }
}

