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
        try
        {
            var result = this.techniqueService.GetTechniques();
            return this.Ok(result.ToList());
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving techniques list");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve techniques list",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving techniques list");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving techniques list",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpGet("gettechnique")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Tecnica> GetTechnique(int techinqueId)
    {
        try
        {
            var result = this.techniqueService.GetTechnique(techinqueId);
            if (result == null)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Technique with ID {techinqueId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving technique {TechniqueId}", techinqueId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve technique",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving technique {TechniqueId}", techinqueId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving the technique",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpDelete("deletetechnique/{techId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteTech(int techId)
    {
        try
        {
            var result = this.techniqueService.DeleteTechnique(techId);
            if (!result)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Technique with ID {techId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error deleting technique {TechniqueId}", techId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to delete technique",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error deleting technique {TechniqueId}", techId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while deleting the technique",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
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

        try
        {
            var result = this.techniqueService.UpdateTechnique(post);
            if (result == null)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Technique with ID {post.idTecnica} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error updating technique {TechniqueId}", post.idTecnica);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to update technique",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error updating technique {TechniqueId}", post.idTecnica);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while updating the technique",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
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

        try
        {
            var result = postRequest.IdTech != 0 ?
                this.techniqueService.UpdateTechnique(postRequest) :
                this.techniqueService.AddTechnique(postRequest);
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error {Action} technique {TechniqueId}",
                postRequest.IdTech != 0 ? "updating" : "adding",
                postRequest.IdTech);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = $"Unable to {(postRequest.IdTech != 0 ? "update" : "add")} technique",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error {Action} technique {TechniqueId}",
                postRequest.IdTech != 0 ? "updating" : "adding",
                postRequest.IdTech);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = $"An unexpected error occurred while {(postRequest.IdTech != 0 ? "updating" : "adding")} the technique",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
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
}
