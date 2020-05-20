namespace UCEME.Seguridad
{
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Web.Security;

    public class CustomIdentity : ICustomIdentity
    {
        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Roles { get; set; }

        public static int GetIdUsuario(string nombre)
        {
            var id = -1;
            using (var db = new Uceme.Model.Models.UCEMEDbEntities())
            {
                var firstOrDefault = db.Usuario.FirstOrDefault(o => o.login == nombre);
                if (firstOrDefault != null)
                {
                    id = firstOrDefault.idUsuario;
                }
            }

            return id;
        }

        public static bool TieneRol(string rol)
        {
            var usp1 = (CustomIdentity)CustomIdentity.FromJson(FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData);

            var res = usp1.IsInRole(rol);
            return res;
        }

        public static ICustomIdentity GetCustomIdentity(string usuario, string password)
        {
            var identity = new CustomIdentity();

            using (var db = new Uceme.Model.Models.UCEMEDbEntities())
            {
                //obtenemos el Hash SHA1 de la password para la busqueda en la bbdd
                var pwSha = Utilidades.Encodificacion.GetSha1(password);
                var user = db.Usuario.FirstOrDefault(o => o.login == usuario && o.password == pwSha);
                if (user != null)
                {
                    identity.IsAuthenticated = true;
                    identity.Name = user.nombre;
                    identity.Email = user.login;
                    identity.Roles = user.Rol.nombre;
                }
            }

            return identity;
        }

        public bool IsInRole(string rol)
        {
            if (rol != this.Roles)
            {
                return false;
            }

            return true;
        }

        public static ICustomIdentity FromJson(string cookie)
        {
            IdentityRepresentation identityRepresentation;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(cookie)))
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(IdentityRepresentation));

                identityRepresentation = jsonSerializer.ReadObject(stream) as IdentityRepresentation;
            }

            CustomIdentity customIdentity = null;
            if (identityRepresentation != null)
            {
                customIdentity = new CustomIdentity
                {
                    IsAuthenticated = identityRepresentation.IsAuthenticated,
                    Name = identityRepresentation.Name,
                    Email = identityRepresentation.Email,
                    Roles = identityRepresentation.Roles,
                    AuthenticationType = "custom"
                };
            }

            return customIdentity;
        }

        public string ToJson()
        {
            string result;

            var ir = new IdentityRepresentation
            {
                IsAuthenticated = IsAuthenticated,
                Name = Name,
                Email = Email,
                Roles = Roles
            };

            var jsonSerializer = new DataContractJsonSerializer(typeof(IdentityRepresentation));

            using (var stream = new MemoryStream())
            {
                jsonSerializer.WriteObject(stream, ir);
                stream.Flush();
                var json = stream.ToArray();
                result = Encoding.UTF8.GetString(json, 0, json.Length);
            }

            return result;
        }
    }
}