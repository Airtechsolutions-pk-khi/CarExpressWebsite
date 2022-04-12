using System.Web.Mvc;
using System.Web.Routing;

namespace CarExpress
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = false;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("styles/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("cache/{*pathInfo}");
            routes.IgnoreRoute("css/{*pathInfo}");
            routes.IgnoreRoute("js/{*pathInfo}");
            routes.IgnoreRoute("fonts/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapRoute(
                "Default",
                "{*AccessUrl}"
            ).RouteHandler = new AccessUrlRouteHandler();
        }
    }
}
