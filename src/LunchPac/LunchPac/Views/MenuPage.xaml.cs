using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LunchPac
{
    public partial class MenuPage : ContentPage
    {
        public MenuViewModel MenuViewModel { get; set; }

        public MenuPage(MenuViewModel vm)
        {
            InitializeComponent();
            BindingContext = MenuViewModel = vm;
        }

        protected override void OnAppearing()
        {
            MenuViewModel.Refresh();
            base.OnAppearing();
        }

        public void OnCloseClicked(object sender, EventArgs e)
        {
            MenuViewModel.HandleCloseClicked();
        }
    }
}

