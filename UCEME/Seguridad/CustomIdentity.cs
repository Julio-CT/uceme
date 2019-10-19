using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Security;

namespace UCEME.Seguridad
{
    public class CustomIdentity : ICustomIdentity
    {
        public static int GetIdUsuario(String nombre)
        {
            var id = -1;
            using (var db = new UCEME.Models.UCEMEDbEntities())
            {
                var firstOrDefault = db.Usuario.FirstOrDefault(o => o.login == nombre);
                if (firstOrDefault != null)
                {
                    id = firstOrDefault.idUsuario;
                }
            }

            return id;
        }

        public static Boolean TieneRol(String rol)
        {
            var usp1 = (CustomIdentity)CustomIdentity.FromJson(FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData);

            var res = usp1.IsInRole(rol);
            return res;
        }

        public static ICustomIdentity GetCustomIdentity(String usuario, string password)
        {
            var identity = new CustomIdentity();

            using (var db = new UCEME.Models.UCEMEDbEntities())
            {
                //obtenemos el Hash SHA1 de la password para la busqueda en la bbdd
                var pwSha = Utilidades.Encodificacion.GetSha1(password);
                var us = db.Usuario.FirstOrDefault(o => o.login == usuario && o.password == pwSha);
                //var us = db.Usuario.FirstOrDefault(o => o.login == usuario && o.password == password);
                if (us != null)
                {
                    identity.IsAuthenticated = true;
                    //voy a probar a cambiar el Login.email por el nombre...a ver si todo va bien :)
                    //identity.Name = us.Login.email;
                    identity.Name = us.nombre;
                    identity.Email = us.login;
                    identity.Roles = us.Rol.nombre;
                }
            }

            return identity;
        }

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Roles { get; set; }

        public bool IsInRole(string rol)
        {
            if (rol != Roles)
            {
                return false;
            }

            return true;
        }

        public static ICustomIdentity FromJson(String cookie)
        {
            IdentityRepresentation ir;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(cookie)))
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(IdentityRepresentation));

                ir = jsonSerializer.ReadObject(stream) as IdentityRepresentation;
            }

            CustomIdentity cu = null;
            if (ir != null)
            {
                cu = new CustomIdentity
                {
                    IsAuthenticated = ir.IsAuthenticated,
                    Name = ir.Name,
                    Email = ir.Email,
                    Roles = ir.Roles,
                    AuthenticationType = "custom"
                };
            }

            return cu;
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