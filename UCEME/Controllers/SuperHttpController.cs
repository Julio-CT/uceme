using System.Web.Http;
using Uceme.Model.Models;

namespace UCEME.Controllers
{
    public class SuperHttpController : ApiController
    {
        protected UCEMEDbEntities DbContext = new UCEMEDbEntities();

        protected override void Dispose(bool disposing)
        {
            this.DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}