namespace UCEME.Seguridad
{
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Security;

    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal()
        {
        }

        public CustomPrincipal(ICustomIdentity identity)
        {
            this.Identity = identity;
        }

        public System.Security.Principal.IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("El rol es nulo");
            }

            return ((ICustomIdentity)this.Identity).IsInRole(role);
        }

        public static void LogOut()
        {
            //la autenticacion de .NET va a crear una cookie
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null)
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
            }

            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(""), new string[] { });
        }

        //para logearse a traves de usuario y password
        public static bool Login(string usuario, string password, bool recordar)
        {
            var identity = CustomIdentity.GetCustomIdentity(usuario, password);

            if (identity.IsAuthenticated)
            {
                HttpContext.Current.User = new CustomPrincipal(identity);
                var ticket = new FormsAuthenticationTicket(1, identity.Name, DateTime.Now, DateTime.Now.AddMinutes(30), recordar, identity.ToJson(), FormsAuthentication.FormsCookiePath);

                var ticketCifrado = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketCifrado)
                {
                    Path = FormsAuthentication.FormsCookiePath
                };

                if (recordar)
                {
                    cookie.Expires = DateTime.Now.AddYears(1);
                }

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            return identity.IsAuthenticated;
        }

        //para logearse a traves de cookie
        public static bool Login(string cookie)
        {
            var identity = CustomIdentity.FromJson(cookie);

            if (identity.IsAuthenticated)
            {
                HttpContext.Current.User = new CustomPrincipal(identity);
            }

            return identity.IsAuthenticated;
        }
    }
}