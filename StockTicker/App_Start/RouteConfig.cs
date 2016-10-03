// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace StockTicker
{
    /// <summary>Manages routes for the mvc application.</summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
