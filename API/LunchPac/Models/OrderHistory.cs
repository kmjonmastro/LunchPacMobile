using System;
using LunchPac.Attributes;

namespace LunchPac.Models
{
    [Table("OrderHistory")]
    public class OrderHistory
    {
        [PrimaryKey]
        public int OrderHistoryId { get; set; }
        public DateTime AddDate { get; set; }
    }
}
