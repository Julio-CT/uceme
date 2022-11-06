﻿namespace UCEME.Utilities
{
    using System;
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
                + (Convert.ToDecimal(strhora.Substring(strhora.Length - 2, 2), CultureInfo.CurrentCulture) / 60);

            return result;
        }

        public static string TimeToString(decimal dateAsInt)
        {
            var hours = Convert.ToInt32(Math.Floor(dateAsInt));
            var minutes = Convert.ToInt32((dateAsInt - hours) * 60);

            hours = minutes == 60 ? hours + 1 : hours;
            minutes = minutes == 60 ? 0 : minutes;

            return hours.ToString("00", CultureInfo.CurrentCulture) + ":" + minutes.ToString("00", CultureInfo.CurrentCulture);
        }
    }
}
