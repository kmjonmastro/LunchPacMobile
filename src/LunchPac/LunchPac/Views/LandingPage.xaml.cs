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

        public  void RestaurantSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("RestaurantSelected");
        }

        public void OrderSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OrderSelected");
        
        }
    }
}

