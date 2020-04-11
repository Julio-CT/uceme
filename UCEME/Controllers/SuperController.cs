﻿using System.Web.Mvc;
using Uceme.Model.Models;

namespace UCEME.Controllers
{
    public class SuperController : Controller
    {
        //
        // GET: /Supercontrolador/

        protected UCEMEDbEntities DbContext = new UCEMEDbEntities();

        protected override void Dispose(bool disposing)
        {
            this.DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}