using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

namespace Uceme.Library.Services;

public class TechniqueService : ITechniqueService
{
    private readonly ILogger<TechniqueService> logger;
    private readonly ApplicationDbContext context;

    public TechniqueService(ILogger<TechniqueService> logger, IApplicationDbContext context)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.context = (ApplicationDbContext)context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Tecnica> GetTechniques()
    {
        return this.ExecuteWithDataExceptionHandling(
            () => this.context.Tecnica,
            "Error retrieving Techniques");
    }

    public Tecnica GetTechnique(int techniqueId)
    {
        return this.ExecuteWithDataExceptionHandling(
            () => this.context.Tecnica.First(x => x.idTecnica == techniqueId),
            "Error retrieving Techniques");
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
