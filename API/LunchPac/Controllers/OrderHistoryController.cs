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
    }
}