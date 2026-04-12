using System;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

namespace Uceme.Library.Mapper;

public class TurnMapper
{
    public static Turn FromTurno(Turno turno)
    {
        if (turno == null)
        {
            throw new ArgumentNullException(nameof(turno));
        }

        return new Turn
        {
            IdTurno = turno.idTurno,
            Dia = turno.dia,
            Inicio = Foundation.Utilities.DateTimeUtils.TimeToString(turno.inicio),
            Fin = Foundation.Utilities.DateTimeUtils.TimeToString(turno.fin),
            Paralelas = turno.paralelas,
            Porhora = turno.porhora,
            IdHospital = turno.idHospital,
        };
    }

    public static Turno ToTurno(Turn turn)
    {
        if (turn == null)
        {
            throw new ArgumentNullException(nameof(turn));
        }

        return new Turno
        {
            idTurno = turn.IdTurno,
            dia = turn.Dia,
            inicio = Foundation.Utilities.DateTimeUtils.TimeToDecimal(turn.Inicio ?? "0"),
            fin = Foundation.Utilities.DateTimeUtils.TimeToDecimal(turn.Fin ?? "0"),
            paralelas = turn.Paralelas,
            porhora = turn.Porhora,
            idHospital = turn.IdHospital,
        };
    }
}
