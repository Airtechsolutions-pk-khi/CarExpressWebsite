using System.Web;
using CarExpress.Helpers;

namespace CarExpress
{
    public class AccessUrlRouteHandler : System.Web.Mvc.MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            if (System.Web.Mvc.AjaxRequestExtensions.IsAjaxRequest(requestContext.HttpContext.Request))
            {
                requestContext.RouteData.Values["controller"] = "Ajax";
                requestContext.RouteData.Values["action"] = "Page";
            }
            else
            {
                requestContext.RouteData.Values["controller"] = "Home";
                requestContext.RouteData.Values["action"] = "Page";
            }
            string access_url = ApplicationHelper.ParseString(requestContext.RouteData.Values["AccessUrl"]);
            requestContext.RouteData.Values["url"] = access_url;
            return base.GetHttpHandler(requestContext);
        }
    }
}