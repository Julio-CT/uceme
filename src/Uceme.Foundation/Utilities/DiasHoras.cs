
namespace UCEME.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public static class DateTimeUtils
    {
        public static decimal TimeToDecimal(string strhora)
        {
            if (strhora == null)
            {
                throw new ArgumentNullException(nameof(strhora));
            }

            decimal result = Convert.ToInt32(strhora.Substring(0, strhora.Length - 3), CultureInfo.CurrentCulture)
                + Convert.ToDecimal(strhora.Substring(strhora.Length - 2, 2), CultureInfo.CurrentCulture) / 60;

            return result;
        }

        public static string TimeToString(decimal inicio)
        {
            var hours = Convert.ToInt32(Math.Floor(inicio));
            var minutes = Convert.ToInt32((inicio - hours) * 60);

            hours = minutes == 60 ? hours + 1 : hours;
            minutes = minutes == 60 ? 0 : minutes;

            return hours.ToString("00", CultureInfo.CurrentCulture) + ":" + minutes.ToString("00", CultureInfo.CurrentCulture);
        }

        public static string WeekDayName(int dia)
        {
            var days = new Dictionary<int, string> { { 1, "Lunes" }, { 2, "Martes" }, { 3, "Miercoles" }, { 4, "Jueves" }, { 5, "Viernes" }, { 6, "Sabado" }, { 7, "Domingo" } };

            return days[dia];
        }

        public static string MonthName(int dia)
        {
            var months = new Dictionary<int, string> { { 1, "Ene" }, { 2, "Feb" }, { 3, "Marzo" }, { 4, "Abr" }, { 5, "May" }, { 6, "Jun" }, { 7, "Jul" }, { 8, "Ago" }, { 9, "Sept" }, { 10, "Oct" }, { 11, "Nov" }, { 12, "Dic" } };

            return months[dia];
        }

        public static string EuropeanDay(int dia)
        {
            var date = dia.ToString(CultureInfo.CurrentCulture);
            if (date.Length == 8)
            {
                date = date.Substring(6, 2) + "-" + date.Substring(4, 2) + "-" + date.Substring(0, 4);
            }
            else
            {
                date = "fecha invalida";
            }

            return date;
        }
    }
}