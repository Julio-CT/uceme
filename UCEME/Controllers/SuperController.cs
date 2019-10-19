using System.Web.Mvc;
using UCEME.Models;

namespace UCEME.Controllers
{
    public class SuperController : Controller
    {
        //
        // GET: /Supercontrolador/

        protected UCEMEDbEntities DbContext = new UCEMEDbEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //introducir la logica de seguridad (comprobacion de usuario, rol o lo que sea)
            base.OnActionExecuting(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}