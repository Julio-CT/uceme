
namespace UCEME.Utilidades
{
    using System;
    using System.Collections.Generic;

    public static class DiasHoras
    {
        public static decimal TimeToDecimal(string strhora)
        {
            decimal dechora = Convert.ToInt32(strhora.Substring(0, strhora.Length - 3)) + Convert.ToDecimal(strhora.Substring(strhora.Length - 2, 2)) / 60;
            return dechora;
        }

        public static string TimeToString(decimal inicio)
        {
            var horas = Convert.ToInt32(Math.Floor(inicio));
            var minutos = Convert.ToInt32((inicio - horas) * 60);
            return horas.ToString("00") + ":" + minutos.ToString("00");
        }

        public static string WeekDay(int dia)
        {
            var dias = new Dictionary<int, string> { { 1, "Lunes" }, { 2, "Martes" }, { 3, "Miercoles" }, { 4, "Jueves" }, { 5, "Viernes" }, { 6, "Sabado" }, { 7, "Domingo" } };

            return dias[dia];
        }

        public static string EuropeanDay(int dia)
        {
            var fecha = dia.ToString();
            if (fecha.Length == 8)
            {
                fecha = fecha.Substring(6, 2) + "-" + fecha.Substring(4, 2) + "-" + fecha.Substring(0, 4);
            }
            else
            {
                fecha = "fecha invalida";
            }
            return fecha;
        }

        public static string MonthName(int dia)
        {
            var meses = new Dictionary<int, string> { { 1, "Ene" }, { 2, "Feb" }, { 3, "Marzo" }, { 4, "Abr" }, { 5, "May" }, { 6, "Jun" }, { 7, "Jul" }, { 8, "Ago" }, { 9, "Sept" }, { 10, "Oct" }, { 11, "Nov" }, { 12, "Dic" } };

            return meses[dia];
        }
    }
}