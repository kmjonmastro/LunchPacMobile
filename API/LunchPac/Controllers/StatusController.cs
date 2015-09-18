using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LunchPac.Models;
using LunchPac.Repositories;

namespace LunchPac.Controllers
{
    public class StatusController : ApiController
    {
        public HttpResponseMessage Get(DateTime date)
        {
            var history = Repository<OrderHistory>.SelectLast();
            var closed = (history != null && history.AddDate.Date == date.Date);

            return Request.CreateResponse(HttpStatusCode.OK, new { Closed = closed });
        }
    }
}