namespace UCEME
{
    using System.Web.Mvc;
    using System.Web.Routing;

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

            routes.MapRoute(
                name: "SEO", // Route name
                url: "{controller}/{action}/{id}/{ignoreThisBit}",
                defaults: new
                {
                    controller = "Blog",
                    action = "SinglePost",
                    id = "",
                    ignoreThisBit = ""
                });
        }
    }
}