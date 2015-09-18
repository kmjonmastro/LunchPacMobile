using System;

namespace LunchPac
{
    public static class Configuration
    {
        public const string DbName = "lunchpac";

        public const string BaseUrl = "http://lunchpac.marathondata.com/";

        public const string LoginUrl = BaseUrl + "api/login";

        public static class CacheKeys
        {
            public const string UserToken = "UserToken";
        }
    }
}

