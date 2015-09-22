using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LunchPac
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryViewModel HistoryViewModel { get; set; }

        public HistoryPage(HistoryViewModel history)
        {
            InitializeComponent();
        }

        void OnItemSelected(object sender, EventArgs e)
        {
            var item = ((ListView)sender).SelectedItem;
            ((ListView)sender).SelectedItem = null;
            HistoryViewModel.HandleHistoryItemClicked(item as Order);
        }
    }
}

