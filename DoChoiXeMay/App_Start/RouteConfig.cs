using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoChoiXeMay
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "DangNhap",
               url: "Login",
               defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional },
               namespaces: new[] { "DoChoiXeMay.Controllers" }
           );
            routes.MapRoute(
               name: "DangNhapWeb",
               url: "LoginWeb",
               defaults: new { controller = "Home", action = "LoginWebGuest", id = UrlParameter.Optional },
               namespaces: new[] { "DoChoiXeMay.Controllers" }
           );
            routes.MapRoute(
               name: "BaoHanh",
               url: "warranty",
               defaults: new { controller = "Active", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "DoChoiXeMay.Controllers" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] {"DoChoiXeMay.Controllers"}
            );
        }
    }
}
