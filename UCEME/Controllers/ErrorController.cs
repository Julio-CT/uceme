namespace UCEME.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            this.Response.StatusCode = 404;  //you may want to set this to 200
            return this.View("NotFound");
        }

        public ActionResult GenericError()
        {
            this.Response.StatusCode = 500;  //you may want to set this to 200
            return this.View("GenericError");
        }
    }
}