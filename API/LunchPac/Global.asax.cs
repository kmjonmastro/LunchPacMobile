using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LunchPac.App_Start;
using Newtonsoft.Json;

namespace LunchPac
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            HttpConfiguration configuration = GlobalConfiguration.Configuration;

            configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ClientReadyJsonResolver()
                };
        }
    }
}