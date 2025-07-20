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
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                var turnos = this.context.Turno.OrderByDescending(a => a.idTurno).ThenByDescending(a => a.dia);
                return turnos.Select(t => Uceme.Library.Mapper.TurnMapper.FromTurno(t));
            },
            "Error retrieving Turnos");
    }

    public Turn GetTurno(int turnoId)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                var turno = this.context.Turno.First(x => x.idTurno == turnoId);
                return Uceme.Library.Mapper.TurnMapper.FromTurno(turno);
            },
            "Error retrieving Turno");
    }

    public Turn CreateTurno(Turn turn)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                if (turn == null)
                {
                    throw new ArgumentNullException(nameof(turn));
                }

                var turno = Uceme.Library.Mapper.TurnMapper.ToTurno(turn);
                this.context.Turno.Add(turno);
                this.context.SaveChanges();

                return Uceme.Library.Mapper.TurnMapper.FromTurno(turno);
            },
            "Error creating Turno");
    }

    public Turn UpdateTurno(int turnoId, Turn turn)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
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
            },
            "Error updating Turno");
    }

    public bool DeleteTurno(int turnoId)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                var turno = this.context.Turno.FirstOrDefault(x => x.idTurno == turnoId);
                if (turno == null)
                {
                    throw new KeyNotFoundException($"Turno with ID {turnoId} not found");
                }

                this.context.Turno.Remove(turno);
                this.context.SaveChanges();

                return true;
            },
            "Error deleting Turno");
    }

    public IEnumerable<Turn> GetTurnosByHospital(int hospitalId)
    {
        return this.ExecuteWithDataExceptionHandling(
            () =>
            {
                var turnos = this.context.Turno.Where(x => x.idHospital == hospitalId);
                return turnos.Select(t => Uceme.Library.Mapper.TurnMapper.FromTurno(t));
            },
            "Error retrieving Turnos for Hospital");
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
