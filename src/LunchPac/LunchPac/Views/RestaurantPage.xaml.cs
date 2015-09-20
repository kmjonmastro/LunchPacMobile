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

        protected override void OnAppearing()
        {
            RestaurantViewModel.Refresh();
            base.OnAppearing();
        }

        void OnOrderButtonClicked(object sender, EventArgs e)
        {
            RestaurantViewModel.OrderButtonClicked();
        }
    }
}

