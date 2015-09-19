using System;

namespace LunchPac
{
    public static class Configuration
    {
        public const string DbName = "lunchpac";

        public static class Routes
        {
            public const string BaseUrl = "http://lunchpac.marathondata.com/";

            public const string LoginUrl = BaseUrl + "api/login";

            public const string RestaurantsUrl = BaseUrl + "api/restaurant";

            public const string OrderhistoryUrl = BaseUrl + "api/orderhistory";

            public const string Order = BaseUrl + "api/order";

            public const string OrderingStatus = BaseUrl + "api/status";
        }

        public static class CacheKeys
        {
            public const string UserToken = "UserToken";
            public const string Restaurants = "Restaurants";
            public const string User = "User";
            public const string OrderHistory = "OrderHistory";
        }
    }
}

