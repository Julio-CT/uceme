using System.Linq;
using Uceme.Model.Models;

namespace UCEME.Utilidades
{
    public static class ConsultasBbdd
    {
        public static Usuario GetUsuariobyId(int id)
        {
            Usuario usu;
            using (var db = new UCEMEDbEntities())
            {
                usu = db.Usuario.FirstOrDefault(o => o.idUsuario == id);
            }

            return usu;
        }
    }
}