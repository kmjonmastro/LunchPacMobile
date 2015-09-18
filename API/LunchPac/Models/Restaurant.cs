using LunchPac.Attributes;

namespace LunchPac.Models
{
    [Table("Restaurants")]
    public class Restaurant
    {
        [PrimaryKey]
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public string Menu { get; set; }
    }
}
