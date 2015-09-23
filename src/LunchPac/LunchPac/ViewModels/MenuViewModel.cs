using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
namespace LunchPac
{
    public class MenuViewModel : ViewModelBase
    {
        readonly INavigator Navigator;

        Restaurant Restaurant { get; set;}

        public string _MenuUrl;
        public string MenuUrl { get { return _MenuUrl; } set { SetRaiseIfPropertyChanged(ref _MenuUrl, value); } }

        public MenuViewModel(INavigator navigator)
        {
            Navigator = navigator;
        }

        public void Refresh()
        {
            if (Restaurant != null)
                SetRestaurant(Restaurant);
        }

        public void SetRestaurant(Restaurant rest)
        {
            MenuUrl = rest.GetMenuUrl();
        }

        public void HandleCloseClicked()
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    Navigator.PopModalAsync().ConfigureAwait(false);
                });
        }
    }
}

