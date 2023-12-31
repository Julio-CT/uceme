namespace Uceme.Library.Services;

using System.Collections.Generic;
using Uceme.Model.Models;

public interface IScheduleService
{
    IEnumerable<Turno> GetTurns();
}
