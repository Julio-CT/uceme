namespace Uceme.Library.Services;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.DataContracts;

public class TurnoService : ITurnoService
{
    private readonly ILogger<TurnoService> logger;
    private readonly ApplicationDbContext context;

    public TurnoService(ILogger<TurnoService> logger, IApplicationDbContext context)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.context = (ApplicationDbContext)context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Turn> GetTurnos()
    {
        try
        {
            var turnos = this.context.Turno.OrderByDescending(a => a.idTurno).ThenByDescending(a => a.dia);
            return turnos.Select(t => Uceme.Library.Mapper.TurnMapper.FromTurno(t));
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving Turnos {EMessage}", e.Message);
            throw new DataException("Error retrieving Turnos", e);
        }
    }

    public Turn GetTurno(int turnoId)
    {
        try
        {
            var turno = this.context.Turno.First(x => x.idTurno == turnoId);
            return Uceme.Library.Mapper.TurnMapper.FromTurno(turno);
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving Turno {EMessage}", e.Message);
            throw new DataException("Error retrieving Turno", e);
        }
    }

    public Turn CreateTurno(Turn turn)
    {
        try
        {
            if (turn == null)
            {
                throw new ArgumentNullException(nameof(turn));
            }

            var turno = Uceme.Library.Mapper.TurnMapper.ToTurno(turn);
            this.context.Turno.Add(turno);
            this.context.SaveChanges();

            return Uceme.Library.Mapper.TurnMapper.FromTurno(turno);
        }
        catch (Exception e)
        {
            this.logger.LogError("Error creating Turno {EMessage}", e.Message);
            throw new DataException("Error creating Turno", e);
        }
    }

    public Turn UpdateTurno(int turnoId, Turn turn)
    {
        try
        {
            if (turn == null)
            {
                throw new ArgumentNullException(nameof(turn));
            }

            var existingTurno = this.context.Turno.FirstOrDefault(x => x.idTurno == turnoId);
            if (existingTurno == null)
            {
                throw new KeyNotFoundException($"Turno with ID {turnoId} not found");
            }

            var updatedTurno = Uceme.Library.Mapper.TurnMapper.ToTurno(turn);
            existingTurno.dia = updatedTurno.dia;
            existingTurno.inicio = updatedTurno.inicio;
            existingTurno.fin = updatedTurno.fin;
            existingTurno.paralelas = updatedTurno.paralelas;
            existingTurno.porhora = updatedTurno.porhora;
            existingTurno.idHospital = updatedTurno.idHospital;

            this.context.SaveChanges();

            return Uceme.Library.Mapper.TurnMapper.FromTurno(existingTurno);
        }
        catch (Exception e)
        {
            this.logger.LogError("Error updating Turno {EMessage}", e.Message);
            throw new DataException("Error updating Turno", e);
        }
    }

    public bool DeleteTurno(int turnoId)
    {
        try
        {
            var turno = this.context.Turno.FirstOrDefault(x => x.idTurno == turnoId);
            if (turno == null)
            {
                throw new KeyNotFoundException($"Turno with ID {turnoId} not found");
            }

            this.context.Turno.Remove(turno);
            this.context.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error deleting Turno {EMessage}", e.Message);
            throw new DataException("Error deleting Turno", e);
        }
    }

    public IEnumerable<Turn> GetTurnosByHospital(int hospitalId)
    {
        try
        {
            var turnos = this.context.Turno.Where(x => x.idHospital == hospitalId);
            return turnos.Select(t => Uceme.Library.Mapper.TurnMapper.FromTurno(t));
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving Turnos for Hospital {EMessage}", e.Message);
            throw new DataException("Error retrieving Turnos for Hospital", e);
        }
    }
}
