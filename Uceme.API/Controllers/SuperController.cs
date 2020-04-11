namespace Uceme.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Uceme.Model.Models;

    public class SuperController : Controller
    {
        protected UCEMEDbEntities DbContext = new UCEMEDbEntities();

        protected override void Dispose(bool disposing)
        {
            this.DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}