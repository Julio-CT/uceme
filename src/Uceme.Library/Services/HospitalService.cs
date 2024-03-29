﻿namespace Uceme.Library.Services;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.Models;

public class HospitalService : IHospitalService
{
    private readonly ILogger<HospitalService> logger;

    private readonly ApplicationDbContext context;

    public HospitalService(ILogger<HospitalService> logger, IApplicationDbContext context)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.context = (ApplicationDbContext)context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<DatosProfesionales> GetHospitals()
    {
        try
        {
            IQueryable<DatosProfesionales> data = this.context.DatosProfesionales.Where(x => x.activo != null && x.activo.Value);

            return data;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving Hospitals {EMessage}", e.Message);
            throw new DataException("Error retrieving Hospitals", e);
        }
    }

    public DatosProfesionales GetHospital(int hospitalId)
    {
        try
        {
            return this.context.DatosProfesionales.First(x => x.idDatosPro == hospitalId);
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving Hospital {EMessage}", e.Message);
            throw new DataException("Error retrieving Hospital", e);
        }
    }
}
