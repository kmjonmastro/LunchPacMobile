using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PestPacMobile
{
    public partial class ApptListPage : ContentPage
    {
        public ApptListViewModel ApptListViewModel { get; set; }

        public ApptListPage(ApptListViewModel apptListViewModel)
        {
            InitializeComponent();
            BindingContext = ApptListViewModel = apptListViewModel;
        }
    }
}

