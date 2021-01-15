﻿using System.Web.Mvc;

namespace UCEME.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }

        public ActionResult GenericError()
        {
            Response.StatusCode = 500;  //you may want to set this to 200
            return View("GenericError");
        }
    }
}