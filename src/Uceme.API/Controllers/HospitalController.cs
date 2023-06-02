namespace Uceme.API.Controllers;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uceme.Library.Services;
using Uceme.Model.Models;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class HospitalController : Controller
{
    private readonly ILogger<HospitalController> logger;
    private readonly IHospitalService hospitalService;

    public HospitalController(
        ILogger<HospitalController> logger,
        IHospitalService hospitalService)
    {
        this.logger = logger;
        this.hospitalService = hospitalService;
    }

    [HttpGet("gethospitals")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<DatosProfesionales>> GetHospitals()
    {
        IEnumerable<DatosProfesionales> result;
        try
        {
            result = this.hospitalService.GetHospitals();
        }
        catch (DataException)
        {
            this.logger.LogError("error getting hospitals");
            return this.BadRequest();
        }

        return result.ToList();
    }

    [HttpGet("gethospital")]
    [AllowAnonymous]
    public ActionResult<DatosProfesionales> GetHospital(int hostpitalId)
    {
        DatosProfesionales result;
        try
        {
            result = this.hospitalService.GetHospital(hostpitalId);
        }
        catch (DataException)
        {
            this.logger.LogError("error getting hospital");
            return this.BadRequest();
        }

        return result;
    }
}
