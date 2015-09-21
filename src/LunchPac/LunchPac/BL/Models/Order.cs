using System;

namespace LunchPac
{
    public class Order
    {
        public int? OrderId { get; set; }

        public int UserId { get; set; }

        public string OrderItem = string.Empty;

        public string Soup = string.Empty;

        public string SoupSize = string.Empty;

        public string OrderComments = string.Empty;

        public DateTime AddDate { get; set; }

        public int RestaurantId { get; set; }
    }
}


