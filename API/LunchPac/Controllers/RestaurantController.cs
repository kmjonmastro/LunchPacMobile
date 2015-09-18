using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LunchPac.Models;
using LunchPac.Repositories;

namespace LunchPac.Controllers
{
    public class RestaurantController : ApiController
    {
        public HttpResponseMessage Get(DateTime date)
        {
            var restaurants = Repository<Restaurant>.SelectAll().OrderBy(r => r.RestaurantName);

            IEnumerable<Restaurant> day = null;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    day = restaurants.Where(r => r.Monday);
                    break;
                case DayOfWeek.Tuesday:
                    day = restaurants.Where(r => r.Tuesday);
                    break;
                case DayOfWeek.Wednesday:
                    day = restaurants.Where(r => r.Wednesday);
                    break;
                case DayOfWeek.Thursday:
                    day = restaurants.Where(r => r.Thursday);
                    break;
                case DayOfWeek.Friday:
                    day = restaurants.Where(r => r.Friday);
                    break;
            }

            return Request.CreateResponse(HttpStatusCode.OK, day);
        }
    }
}