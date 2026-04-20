using System.Collections.Generic;

namespace Uceme.Library.Services;

public interface ITurnoService
{
    IEnumerable<Uceme.Model.DataContracts.Turn> GetTurnos();

    Uceme.Model.DataContracts.Turn GetTurno(int turnoId);

    Uceme.Model.DataContracts.Turn CreateTurno(Uceme.Model.DataContracts.Turn turno);

    Uceme.Model.DataContracts.Turn UpdateTurno(int turnoId, Uceme.Model.DataContracts.Turn turno);

    bool DeleteTurno(int turnoId);

    IEnumerable<Uceme.Model.DataContracts.Turn> GetTurnosByHospital(int hospitalId);
}
