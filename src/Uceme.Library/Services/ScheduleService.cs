namespace Uceme.Library.Services;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.Models;

public class ScheduleService : IScheduleService
{
    private readonly ILogger<ScheduleService> logger;

    private readonly ApplicationDbContext context;

    public ScheduleService(
        ILogger<ScheduleService> logger,
        IApplicationDbContext context)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.context = (ApplicationDbContext)context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Turno> GetTurns()
    {
        try
        {
            IOrderedQueryable<Turno> existingAppointments = this.context.Turno.OrderByDescending(a => a.idTurno).ThenByDescending(a => a.dia);

            return existingAppointments;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving turns {EMessage}", e.Message);
            throw new DataException("Error retrieving turns", e);
        }
    }
}
