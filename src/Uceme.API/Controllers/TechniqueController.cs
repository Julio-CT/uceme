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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Tecnica>> GetTechniques()
    {
        var result = this.HandleControllerOperation(
            () => this.techniqueService.GetTechniques().ToList(),
            "retrieving techniques list");
        return this.Ok(result);
    }

    [HttpGet("gettechnique")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Tecnica> GetTechnique(int techinqueId)
    {
        var result = this.HandleControllerOperation(
            () => this.techniqueService.GetTechnique(techinqueId),
            "retrieving technique",
            techinqueId);
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpDelete("deletetechnique/{techId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteTech(int techId)
    {
        var result = this.HandleControllerOperation(
            () => this.techniqueService.DeleteTechnique(techId),
            "deleting technique",
            techId);
        if (!result)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpPut("updatetech")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Tecnica> UpdateTech([FromBody] Tecnica post)
    {
        if (post is null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Technique data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        var result = this.HandleControllerOperation(
            () => this.techniqueService.UpdateTechnique(post),
            "updating technique",
            post.idTecnica);
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpPost("addtech")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> AddTech([FromBody] TechniqueRequest postRequest)
    {
        if (postRequest == null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Technique request data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        var result = this.HandleControllerOperation(
            () => postRequest.IdTech != 0 ?
                this.techniqueService.UpdateTechnique(postRequest) :
                this.techniqueService.AddTechnique(postRequest),
            postRequest.IdTech != 0 ? "updating" : "adding",
            postRequest.IdTech);
        if (!result)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpPost("ontechuploadasync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<string>> OnTechUploadAsync(IFormFile file)
    {
        if (file is null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "File is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (this.configuration?.Value?.BlogImagesDir == null
            || !Directory.Exists(this.configuration?.Value?.BlogImagesDir))
        {
            return this.StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
            {
                Title = "Configuration Error",
                Detail = "Images directory is not properly configured",
                Status = StatusCodes.Status502BadGateway,
            });
        }

        try
        {
            if (file.Length == 0 || Path.GetExtension(file.FileName).Length >= 5)
            {
                return this.BadRequest(new ProblemDetails
                {
                    Title = "Invalid File",
                    Detail = "File is empty or has an invalid extension",
                    Status = StatusCodes.Status400BadRequest,
                });
            }

            var filename = "Blog" + this.techniqueService.GetNextTechImage() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(this.configuration.Value.BlogImagesDir, filename);

#pragma warning disable CA3003 // Review code for file path injection vulnerabilities
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream).ConfigureAwait(false);
            }
#pragma warning restore CA3003 // Review code for file path injection vulnerabilities

            return this.Ok(filename);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while uploading the file",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    private T HandleControllerOperation<T>(Func<T> operation, string errorContext, object? contextId = null)
    {
        try
        {
            return operation();
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, $"Error {errorContext}", contextId);
            throw new InvalidOperationException($"Unable to {errorContext.ToUpperInvariant()}", ex);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, $"Unexpected error {errorContext}", contextId);
            throw new InvalidOperationException($"An unexpected error occurred while {errorContext.ToUpperInvariant()}", ex);
        }
    }
}
