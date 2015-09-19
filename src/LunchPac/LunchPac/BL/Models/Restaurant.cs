using System;

using Xamarin.Forms;

namespace LunchPac
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; }

        public bool Monday { get; set; }

        public bool Tuesday { get; set; }

        public bool Wednesday { get; set; }

        public bool Thursday { get; set; }

        public bool Friday { get; set; }

        public string Menu { get; set; }

        public string GetMenuUrl()
        {
            return Configuration.Routes.BaseUrl + Menu;
        }
    }
}


