using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LunchPac
{
    public partial class LandingPage : ContentPage
    {
        LandingPageViewModel LandingPageViewModel { get; set; }

        public LandingPage(LandingPageViewModel landingPageViewModel)
        {
            InitializeComponent();
            LandingPageViewModel = landingPageViewModel;
            BindingContext = LandingPageViewModel;
        }


        protected override void OnAppearing()
        {
            LandingPageViewModel.OnAppearing();
            base.OnAppearing();
        }

        public void RestaurantSelected(object sender, EventArgs e)
        {
            var item = ((ListView)sender).SelectedItem;
            ((ListView)sender).SelectedItem = null;
            LandingPageViewModel.HandleRestaurantSelected(item as Restaurant);
        }

        public void OrderSelected(object sender, EventArgs e)
        {
            var item = ((ListView)sender).SelectedItem;
            ((ListView)sender).SelectedItem = null;
            LandingPageViewModel.HandleOrderSelected(item as Order);
        }
    }
}

