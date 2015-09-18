using System.Net;
using System.Net.Http;
using System.Web.Http;
using LunchPac.Models;
using LunchPac.Repositories;

namespace LunchPac.Controllers
{
    public class OrderController : ApiController
    {
        public HttpResponseMessage Get(int orderId)
        {
            var order = Repository<Order>.Select(orderId);
            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        public HttpResponseMessage Post(Order order)
        {
            var orderId = Repository<Order>.Insert(order);
            return Request.CreateResponse(HttpStatusCode.OK, orderId);
        }

        public HttpResponseMessage Put(Order order)
        {
            Repository<Order>.Update(order);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int orderId)
        {
            Repository<Order>.Delete(orderId);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}