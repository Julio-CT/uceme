using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.Models;

namespace Uceme.Library.Services;

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
        return this.ExecuteWithDataExceptionHandling(
            () => this.context.DatosProfesionales.Where(x => x.activo != null && x.activo.Value),
            "Error retrieving Hospitals");
    }

    public DatosProfesionales GetHospital(int hospitalId)
    {
        return this.ExecuteWithDataExceptionHandling(
            () => this.context.DatosProfesionales.First(x => x.idDatosPro == hospitalId),
            "Error retrieving Hospital");
    }

    public DatosProfesionales CreateHospital(DatosProfesionales hospital)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                if (hospital == null)
                {
                    throw new ArgumentNullException(nameof(hospital));
                }

                hospital.activo = true;

                this.context.DatosProfesionales.Add(hospital);
                this.context.SaveChanges();

                return hospital;
            },
            "Error creating Hospital");
    }

    public DatosProfesionales UpdateHospital(int hospitalId, DatosProfesionales hospital)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
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
            },
            "Error updating Hospital");
    }

    public bool DeleteHospital(int hospitalId)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                var hospital = this.context.DatosProfesionales.FirstOrDefault(x => x.idDatosPro == hospitalId);
                if (hospital == null)
                {
                    throw new KeyNotFoundException($"Hospital with ID {hospitalId} not found");
                }

                hospital.activo = false;

                this.context.SaveChanges();

                return true;
            },
            "Error deleting Hospital");
    }

    public bool HardDeleteHospital(int hospitalId)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                var hospital = this.context.DatosProfesionales.FirstOrDefault(x => x.idDatosPro == hospitalId);
                if (hospital == null)
                {
                    throw new KeyNotFoundException($"Hospital with ID {hospitalId} not found");
                }

                this.context.DatosProfesionales.Remove(hospital);
                this.context.SaveChanges();

                return true;
            },
            "Error hard deleting Hospital");
    }

    private T ExecuteWithDataExceptionHandling<T>(Func<T> func, string errorMessage)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            this.logger.LogError("{ErrorMessage} {EMessage}", errorMessage, e.Message);
            throw new DataException(errorMessage, e);
        }
    }
}
