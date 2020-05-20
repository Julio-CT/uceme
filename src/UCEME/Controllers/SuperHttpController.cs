namespace UCEME.Controllers
{
    using System.Web.Http;
    using Uceme.Model.Models;

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