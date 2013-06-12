using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ninesky
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute(
            //    name: "Categoryes",
            //    url: "Categoryes/{id}",
            //    defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional }
            //    );
            //routes.MapRoute(
            //    name: "Items",
            //    url: "Items/{id}",
            //    defaults: new { controller = "Mail", action = "Index", id = UrlParameter.Optional }
            //    );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{page}",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional, page = UrlParameter.Optional }
                );
        }
    }
}