namespace Uceme.Library.Services;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

public class TechniqueService : ITechniqueService
{
    private readonly ILogger<TechniqueService> logger;

    private readonly ApplicationDbContext context;

    public TechniqueService(ILogger<TechniqueService> logger, IApplicationDbContext context)
    {
        this.logger = logger;
        this.context = (ApplicationDbContext)context;
    }

    public IEnumerable<Tecnica> GetTechniques()
    {
        try
        {
            Microsoft.EntityFrameworkCore.DbSet<Tecnica> data = this.context.Tecnica;

            return data;
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error retrieving Techniques {e.Message}");
            throw new DataException("Error retrieving Techniques", e);
        }
    }

    public Tecnica GetTechnique(int techniqueId)
    {
        try
        {
            return this.context.Tecnica.First(x => x.idTecnica == techniqueId);
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error retrieving Techniques {e.Message}");
            throw new DataException("Error retrieving Techniques", e);
        }
    }

    public bool DeleteTechnique(int techId)
    {
        throw new NotImplementedException();
    }

    public Tecnica UpdateTechnique(Tecnica post)
    {
        throw new NotImplementedException();
    }

    public bool UpdateTechnique(TechniqueRequest postRequest)
    {
        throw new NotImplementedException();
    }

    public bool AddTechnique(TechniqueRequest postRequest)
    {
        throw new NotImplementedException();
    }

    public string GetNextTechImage()
    {
        throw new NotImplementedException();
    }
}
