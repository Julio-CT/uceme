using System.Web.Http;
using System.Web.Mvc;
using Uceme.Model.Models;

namespace UCEME.Controllers
{
    public class SuperHttpController : ApiController
    {
        protected UCEMEDbEntities DbContext = new UCEMEDbEntities();

        protected override void Dispose(bool disposing)
        {
            DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}