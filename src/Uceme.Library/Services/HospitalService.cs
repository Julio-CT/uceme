namespace Uceme.Library.Services;

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

    public DatosProfesionales CreateHospital(DatosProfesionales hospital)
    {
        try
        {
            if (hospital == null)
            {
                throw new ArgumentNullException(nameof(hospital));
            }

            hospital.activo = true;

            this.context.DatosProfesionales.Add(hospital);
            this.context.SaveChanges();

            return hospital;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error creating Hospital {EMessage}", e.Message);
            throw new DataException("Error creating Hospital", e);
        }
    }

    public DatosProfesionales UpdateHospital(int hospitalId, DatosProfesionales hospital)
    {
        try
        {
            if (hospital == null)
            {
                throw new ArgumentNullException(nameof(hospital));
            }

            var existingHospital = this.context.DatosProfesionales.FirstOrDefault(x => x.idDatosPro == hospitalId);
            if (existingHospital == null)
            {
                throw new KeyNotFoundException($"Hospital with ID {hospitalId} not found");
            }

            existingHospital.nombre = hospital.nombre;
            existingHospital.direccion = hospital.direccion;

            this.context.SaveChanges();

            return existingHospital;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error updating Hospital {EMessage}", e.Message);
            throw new DataException("Error updating Hospital", e);
        }
    }

    public bool DeleteHospital(int hospitalId)
    {
        try
        {
            var hospital = this.context.DatosProfesionales.FirstOrDefault(x => x.idDatosPro == hospitalId);
            if (hospital == null)
            {
                throw new KeyNotFoundException($"Hospital with ID {hospitalId} not found");
            }

            hospital.activo = false;

            this.context.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error deleting Hospital {EMessage}", e.Message);
            throw new DataException("Error deleting Hospital", e);
        }
    }

    public bool HardDeleteHospital(int hospitalId)
    {
        try
        {
            var hospital = this.context.DatosProfesionales.FirstOrDefault(x => x.idDatosPro == hospitalId);
            if (hospital == null)
            {
                throw new KeyNotFoundException($"Hospital with ID {hospitalId} not found");
            }

            this.context.DatosProfesionales.Remove(hospital);
            this.context.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error hard deleting Hospital {EMessage}", e.Message);
            throw new DataException("Error hard deleting Hospital", e);
        }
    }
}
