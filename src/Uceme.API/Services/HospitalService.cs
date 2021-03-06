﻿namespace Uceme.API.Services
{
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

        private readonly ApplicationDbContext dbContext;

        public HospitalService(ILogger<HospitalService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.dbContext = context;
        }

        public IEnumerable<DatosProfesionales> GetHospitals()
        {
            try
            {
                var data = this.dbContext.DatosProfesionales.Where(x => x.activo != null && x.activo.Value);

                return data;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Hospitals {e.Message}");
                throw new DataException("Error retrieving Hospitals", e);
            }
        }

        public DatosProfesionales GetHospital(int hospitalId)
        {
            try
            {
                return this.dbContext.DatosProfesionales.FirstOrDefault(x => x.idDatosPro == hospitalId);

            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Hospital {e.Message}");
                throw new DataException("Error retrieving Hospital", e);
            }
        }
    }
}
