using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LunchPac
{
    public partial class OrderFormPage : ContentPage
    {
        readonly OrderFormViewModel OrderFormViewModel;

        public OrderFormPage(OrderFormViewModel vm)
        {
            OrderFormViewModel = vm;
            InitializeComponent();
        }

        void SubmitClicked(object sender, EventArgs e)
        {
            OrderFormViewModel.ValidateAndSubmit();
        }

        void DeleteClicked(object sender, EventArgs e)
        {
            OrderFormViewModel.CancelOrder();
        }
    }
}

