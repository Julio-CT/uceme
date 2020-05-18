namespace UCEME
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using UCEME.Seguridad;
    using UCEME.Utilidades;

    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7,
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            MvcHandler.DisableMvcResponseHeader = true; //this line is to hide mvc header
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            this.Response.ClearContent();
            var ex = this.Server.GetLastError().GetBaseException();
            GestionErrores.RegistroErrorEmail(ex.ToString(), ex.Source, "");
            HttpContext.Current.ClearError();
            this.Server.ClearError();
            this.Response.Clear();
            this.Response.Redirect("/Error/GenericError");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (this.Request.IsAuthenticated)
            {
                var usp1 =
                    (CustomIdentity)
                        CustomIdentity.FromJson(
                            FormsAuthentication.Decrypt(
                                HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData);
                var principal = new CustomPrincipal(usp1);
                HttpContext.Current.User = principal;
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
        }
    }
}