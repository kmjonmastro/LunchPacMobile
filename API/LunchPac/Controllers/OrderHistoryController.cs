using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LunchPac.Models;
using LunchPac.Repositories;

namespace LunchPac.Controllers
{
    public class OrderHistoryController : ApiController
    {
        public HttpResponseMessage Get(int userId, int restaurantId)
        {
            var orders = Repository<Order>
                .SelectMany(o => o.UserId, userId)
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.AddDate)
                .GroupBy(o => new { o.OrderItem, o.OrderComments })
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .Take(5);

            return Request.CreateResponse(HttpStatusCode.OK, orders);
        }

        public HttpResponseMessage Get(DateTime date)
        {
            var restaurants = Repository<Restaurant>.SelectAll();
            var users = Repository<User>.SelectAll().ToList();
            var orders = Repository<Order>.SelectByDate(o => o.AddDate, date);

            var history =
                from r in restaurants
                join o in orders on r.RestaurantId equals o.RestaurantId
                join u in users on o.UserId equals u.UserId
                let c = new
                    {
                        u.Name,
                        o.OrderItem,
                        o.Soup,
                        o.SoupSize,
                        o.OrderComments
                    }
                orderby r.RestaurantName, u.Name
                group c by r.RestaurantName into g
                select new
                    {
                        Name = g.Key,
                        Orders = g.ToList()
                    };

            return Request.CreateResponse(HttpStatusCode.OK, history);
        }
    }
}