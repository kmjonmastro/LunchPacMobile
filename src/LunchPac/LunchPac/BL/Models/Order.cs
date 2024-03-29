﻿using System;
using Newtonsoft.Json;

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

        #region Client Side Stuff

        [JsonIgnore]
        public bool IsCurrentOrder { get; set; }

        #endregion

        public Order()
        {
            OrderItem = string.Empty;
            Soup = string.Empty;
            SoupSize = string.Empty;
            OrderComments = string.Empty;
        }
    }
}


