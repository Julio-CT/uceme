using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Uceme.Model.Models;
using WebMatrix.WebData;

namespace UCEME.Filters
{
    public class SimpleMembershipInitializer
    {
        public SimpleMembershipInitializer()
        {
            Database.SetInitializer<UsersContext>(null);

            try
            {
                using (var context = new UsersContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Crear la base de datos SimpleMembership sin el esquema de migración de Entity Framework
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    }
                }

                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo inicializar la base de datos de ASP.NET Simple Membership. Para obtener más información, consulte http://go.microsoft.com/fwlink/?LinkId=256588", ex);
            }
        }
    }
}