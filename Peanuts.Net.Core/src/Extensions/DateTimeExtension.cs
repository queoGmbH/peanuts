namespace Com.QueoFlow.Peanuts.Net.Core.Extensions {
    using System;
using System.Globalization;


    /// <summary>
    ///     Klasse mit Erweiterungsmethoden für DateTime-Instanzen
    /// </summary>
    public static class DateTimeExtensions {
        /// <summary>
        ///     Berechnet das Datum des Montags anhand eines Jahres und der Kalenderwoche
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
        /// via http://stackoverflow.com/questions/662379/calculate-date-from-week-number
        public static DateTime FirstDateOfWeekIso8601(int year, int weekOfYear) {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1) {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        /// <summary>
        ///     Liefert die Anzahl der Tage des Monats des übergebeben Datums.
        /// </summary>
        /// <param name="date"> </param>
        /// <returns> </returns>
        public static int GetDaysInMonth(this DateTime date) {
            DateTime a = date;
            DateTime b = date.AddMonths(1);

            DateTime firstA = GetFirstDayOfMonth(a);
            DateTime firstB = GetFirstDayOfMonth(b);

            return firstB.Subtract(firstA).Days;
        }

        /// <summary>
        ///     Liefert den ersten Tag des Monats in dem sich das Datum befindet.
        /// </summary>
        /// <param name="datetime"> </param>
        /// <returns> </returns>
        public static DateTime GetFirstDayOfMonth(this DateTime datetime) {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        /// <summary>
        ///     Liefert den ersten Tag der Woche in dem sich das Datum befindet.
        /// </summary>
        /// <param name="datetime"> </param>
        /// <returns> </returns>
        public static DateTime GetFirstDayOfWeek(this DateTime datetime) {
            DateTime tmp = datetime;
            while (tmp.DayOfWeek != DayOfWeek.Monday) {
                tmp = tmp.AddDays(-1);
            }

            return tmp;
        }

        /// <summary>
        ///     Ruft den ersten Wochenanfange (derzeit Montag) des Monats vom übergebenen Datum ab.
        /// </summary>
        /// <param name="datetime"> </param>
        /// <returns> </returns>
        public static DateTime GetFirstWeekStartOfMonth(this DateTime datetime) {
            DateTime tmp = datetime.GetFirstDayOfMonth();
            while (tmp.DayOfWeek != DayOfWeek.Monday) {
                tmp = tmp.AddDays(1);
            }
            return tmp;
        }

        /// <summary>
        ///     Ruft das Quartal des Datums ab.
        /// </summary>
        /// <param name="date"> </param>
        /// <returns> </returns>
        public static int GetQuarter(this DateTime date) {
            switch (date.Month) {
                case 1:
                case 2:
                case 3: {
                    return 1;
                }
                case 4:
                case 5:
                case 6: {
                    return 2;
                }
                case 7:
                case 8:
                case 9: {
                    return 3;
                }
                default: {
                    return 4;
                }
            }
        }

        /// <summary>
        ///     Erstellt eine Sekundengenaue Kopie des DateTime-Objektes.
        /// </summary>
        /// <param name="dateTime"> Das originale DateTime-Objekt von dem eine Kopie erstellt werden soll. </param>
        /// <returns> </returns>
        public static DateTime GetSecondsPreciseCopy(this DateTime dateTime) {
            return new DateTime(dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second);
        }

        /// <summary>
        ///     Liefert die Kalenderwoche in der das Datum liegt unter der Annahme
        ///     der <see cref="CalendarWeekRule.FirstFourDayWeek" /> und dem Montag als
        ///     ersten Wochentag.
        ///     Die Implementierung umgeht einen Fehler im Framework beim Jahreswechsel.
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns>Kalenderwoche</returns>
        public static int GetWeekOfYear(this DateTime date) {
            return GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        ///     Liefert die Kalenderwoche in der das Datum liegt.
        ///     Die Implementierung umgeht einen Fehler im Framework beim Jahreswechsel.
        /// </summary>
        /// <param name="date">Datum</param>
        /// <param name="calendarWeekRule">Kalenderwochenregel</param>
        /// <param name="firstDayOfWeek">Erster Wochentag</param>
        /// <returns>Kalenderwoche</returns>
        public static int GetWeekOfYear(this DateTime date, CalendarWeekRule calendarWeekRule, DayOfWeek firstDayOfWeek) {
            DateTime testDate = date;
            if (date.DayOfWeek >= DayOfWeek.Monday && date.DayOfWeek <= DayOfWeek.Wednesday) {
                testDate = date.AddDays(3);
            }
            int calendarweek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(testDate,
                    calendarWeekRule,
                    firstDayOfWeek);
            return calendarweek;
        }

        /// <summary>
        ///     Überprüft ob die zwei Datums-Parameter in derselben Woche liegen.
        /// </summary>
        /// <param name="date"> Erstes Datum </param>
        /// <param name="otherDate"> Zweites Datum </param>
        /// <returns> TRUE wenn die zwei Datums in der gleichen Woche liegen andernfalls FALSE. </returns>
        public static bool IsSameWeek(this DateTime date, DateTime otherDate) {
            return date.Date.GetFirstDayOfWeek().Equals(otherDate.Date.GetFirstDayOfWeek());
        }

        /// <summary>
        ///     Vergleicht ein DateTime Objekt mit einen übergebenen. Liefert das spätere zurück. Sind beide gleich wird das
        ///     Ausgangs DateTime Objekt zurückgegeben.
        /// </summary>
        /// <param name="firstDate"> Das Ausgangs DateTime Objekt </param>
        /// <param name="secondDate"> Das zu prüfende DateTime Objekt </param>
        /// <returns> Das größere DateTime Objekt oder das Ausgangs DateTime Objekt </returns>
        public static DateTime Max(this DateTime firstDate, DateTime secondDate) {
            if (DateTime.Compare(firstDate, secondDate) >= 0) {
                return firstDate;
            }
            return secondDate;
        }

        /// <summary>
        ///     Vergleicht zwei DateTime Objekte. Liefert das frühere zurück. Sind beide gleich wird das Ausgangs DateTime Objekt
        ///     zurückgegeben.
        /// </summary>
        /// <param name="firstDate"> Das Ausgangs DateTime Objekt </param>
        /// <param name="secondDate"> Das zu prüfende DateTime Objekt </param>
        /// <returns> Das kleinere DateTime Objekt oder das Ausgangs DateTime Objekt </returns>
        public static DateTime Min(this DateTime firstDate, DateTime secondDate) {
            if (DateTime.Compare(firstDate, secondDate) <= 0) {
                return firstDate;
            }
            return secondDate;
        }

        /// <summary>
        ///     Überprüft ob der Monat des Datum den ersten Tag der Kalenderwoche des Jahres beinhaltet.
        /// </summary>
        /// <param name="date"> Das Datum welches überprüft werden soll </param>
        /// <param name="cal"> Calenderobjekt mit Kulturinfos </param>
        /// <param name="week"> Die gesuchte Kalenderwoche </param>
        /// <returns> </returns>
        public static bool MonthContainsStartOfWeekOfYear(this DateTime date, Calendar cal, int week) {
            for (DateTime monthDate = date.GetFirstWeekStartOfMonth(); monthDate.Month == date.Month;
                    monthDate = monthDate.AddDays(7).GetFirstDayOfWeek()) {
                int weekOfDate = cal.GetWeekOfYear(monthDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                if (week == weekOfDate) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Liefert zu einem Datum den Monat und das Jahr getrennt mit einem Separator
        /// </summary>
        /// <param name="datetime"> </param>
        /// <param name="separator"> Der Separator; default: " / " </param>
        /// <param name="monthFormatString"> Der FormatString für den Monat </param>
        /// <returns> </returns>
        public static string ToMonthYearString(this DateTime datetime, string separator = " / ",
                string monthFormatString = "{0:00}") {
            return String.Format(monthFormatString, datetime.Month) + separator + datetime.Year;
        }

        
    
}
}