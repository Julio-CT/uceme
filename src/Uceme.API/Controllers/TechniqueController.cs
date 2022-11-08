namespace Uceme.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Uceme.Library.Services;
    using Uceme.Model.DataContracts;
    using Uceme.Model.Models;
    using Uceme.Model.Settings;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TechniqueController : Controller
    {
        private readonly ILogger<BlogController> logger;

        private readonly ITechniqueService techniqueService;

        private readonly IOptions<AppSettings> configuration;

        public TechniqueController(
            ITechniqueService techniqueService,
            IOptions<AppSettings> configuration,
            ILogger<BlogController> logger)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.techniqueService = techniqueService;
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("gettechniques")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Tecnica>> GetTechniques()
        {
            IEnumerable<Tecnica> result;
            try
            {
                result = this.techniqueService.GetTechniques();
            }
            catch (DataException)
            {
                this.logger.LogError("error returning techniques");
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("gettechnique")]
        [AllowAnonymous]
        public ActionResult<Tecnica> GetTechnique(int techinqueId)
        {
            Tecnica result;
            try
            {
                result = this.techniqueService.GetTechnique(techinqueId);
            }
            catch (DataException)
            {
                this.logger.LogError("error returning technique");
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("deletetechnique")]
        public ActionResult<bool> DeleteTech(int techId)
        {
            bool result;
            try
            {
                result = this.techniqueService.DeleteTechnique(techId);
            }
            catch (DataException)
            {
                this.logger.LogError("error deleting technique");
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("updatetech")]
        public ActionResult<Tecnica> UpdateTech(Tecnica post)
        {
            Tecnica result;
            try
            {
                result = this.techniqueService.UpdateTechine(post);
            }
            catch (DataException)
            {
                this.logger.LogError("error updating technique");
                return this.BadRequest();
            }

            return result;
        }

        [HttpPost("addtech")]
        public ActionResult<bool> AddTech([FromBody] TechniqueRequest postRequest)
        {
            if (postRequest == null)
            {
                return this.BadRequest();
            }

            bool result;
            try
            {
                if (postRequest.IdTech != 0)
                {
                    result = this.techniqueService.UpdateTechnique(postRequest);
                }
                else
                {
                    result = this.techniqueService.AddTechnique(postRequest);
                }
            }
            catch (DataException)
            {
                this.logger.LogError("error adding technique");
                return this.BadRequest();
            }

            return result;
        }

        [HttpPost("ontechuploadasync")]
        public async Task<ActionResult<string>> OnTechUploadAsync([FromForm] IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (this.configuration?.Value?.BlogImagesDir == null
                || !Directory.Exists(this.configuration?.Value?.BlogImagesDir))
            {
                return this.StatusCode(StatusCodes.Status502BadGateway);
            }

            string filename = string.Empty;

            try
            {
                if (file.Length > 0 && Path.GetExtension(file.FileName).Length < 5)
                {
                    filename = "Blog" + this.techniqueService.GetNextTechImage() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(
                        this.configuration.Value.BlogImagesDir,
                        filename);

#pragma warning disable CA3003 // Review code for file path injection vulnerabilities
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream).ConfigureAwait(false);
                    }
#pragma warning restore CA3003 // Review code for file path injection vulnerabilities
                }
            }
            catch (Exception)
            {
                this.logger.LogError("error uploading technique");
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return filename;
        }
    }
}
