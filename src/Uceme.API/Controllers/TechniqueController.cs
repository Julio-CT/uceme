namespace Uceme.API.Controllers;

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
    private readonly ILogger<TechniqueController> logger;
    private readonly ITechniqueService techniqueService;
    private readonly IOptions<AppSettings> configuration;

    public TechniqueController(
        ITechniqueService techniqueService,
        IOptions<AppSettings> configuration,
        ILogger<TechniqueController> logger)
    {
        this.techniqueService = techniqueService ?? throw new ArgumentNullException(nameof(techniqueService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
        if (post is null)
        {
            return this.BadRequest($"'{nameof(post)}' cannot be null or empty.");
        }

        Tecnica result;
        try
        {
            result = this.techniqueService.UpdateTechnique(post);
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
            return this.BadRequest($"'{nameof(postRequest)}' cannot be null or empty.");
        }

        bool result;
        try
        {
            result = postRequest.IdTech != 0 ?
             this.techniqueService.UpdateTechnique(postRequest)
             : this.techniqueService.AddTechnique(postRequest);
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
            return this.BadRequest($"'{nameof(file)}' cannot be null or empty.");
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
                string filePath = Path.Combine(
                    this.configuration.Value.BlogImagesDir,
                    filename);

#pragma warning disable CA3003 // Review code for file path injection vulnerabilities
                using (FileStream stream = System.IO.File.Create(filePath))
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
