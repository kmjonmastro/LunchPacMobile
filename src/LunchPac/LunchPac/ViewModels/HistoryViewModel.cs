using System;

namespace LunchPac
{
    public class HistoryViewModel : ViewModelBase
    {
        readonly INavigator Navigator;

        public HistoryViewModel(INavigator Navigator)
        {
            this.Navigator = Navigator;
            this.Title = "Order History";
        }

        public void HandleHistoryItemClicked(Order order)
        {
            var o = order;
        }
    }
}

