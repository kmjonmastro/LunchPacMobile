using System;
using LunchPac.Attributes;

namespace LunchPac.Models
{
    [Table("Orders")]
    public class Order
    {
        [PrimaryKey]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderItem { get; set; }
        public string Soup { get; set; }
        public string SoupSize { get; set; }
        public string OrderComments { get; set; }
        public DateTime AddDate { get; set; }
        public int RestaurantId { get; set; }
        public int FormSection { get; set; }

        public Order()
        {
            FormSection = 1;
        }
    }
}
