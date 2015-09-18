using System;

using Xamarin.Forms;

namespace LunchPac
{
    public class LandingPageViewModel :  ViewModelBase
    {
        readonly INavigator Navigator;

        public LandingPageViewModel(INavigator Navigator)
        {
            this.Navigator = Navigator;
        }


    }
}


