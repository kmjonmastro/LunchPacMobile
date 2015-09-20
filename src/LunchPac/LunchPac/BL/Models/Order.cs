using System;

namespace LunchPac
{
    public class Order
    {
        public int? OrderId { get; set; }

        public int UserId { get; set; }

        public string OrderItem { get; set; }

        public string Soup { get; set; }

        public string SoupSize { get; set; }

        public string OrderComments { get; set; }

        public DateTime AddDate { get; set; }

        public int RestaurantId { get; set; }

        public Order()
        {
            
        }
    }
}


