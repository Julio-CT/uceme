namespace UCEME.Utilidades
{
    using System.Linq;
    using Uceme.Model.Models;

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