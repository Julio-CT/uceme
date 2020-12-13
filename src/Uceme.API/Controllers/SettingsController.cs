namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Uceme.Model.Models;
    using Uceme.Model.Settings;

    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> logger;
    
        private readonly IOptions<AppSettings> configuration;

        public SettingsController(
            ILogger<SettingsController> logger,
            IOptions<AppSettings> configuration)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet("getsettings")]
        [AllowAnonymous]
        public ActionResult<Dictionary<string, object>> GetSettings()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                result["Telephone"] = this.configuration.Value.Telephone;
                result["ContactEmail"] = this.configuration.Value.ContactEmail;
                result["Address"] = this.configuration.Value.Address;
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }
    }
}