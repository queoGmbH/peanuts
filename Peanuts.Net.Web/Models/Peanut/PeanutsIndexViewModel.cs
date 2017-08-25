using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    ///     ViewModel für die Übersichtsseite der Peanuts eines Monats
    /// </summary>
    public class PeanutsIndexViewModel {
        public PeanutsIndexViewModel(int year, int month, IDictionary<DateTime, IList<PeanutParticipation>> peanutParticipations,
            IDictionary<DateTime, IList<Core.Domain.Peanuts.Peanut>> attendablePeanuts) {
            Require.NotNull(peanutParticipations, "peanutParticipations");
            Require.NotNull(attendablePeanuts, "attendablePeanuts");

            Year = year;
            Month = month;

            PeanutParticipations = peanutParticipations;
            AttendablePeanuts = attendablePeanuts;
        }

        /// <summary>
        ///     Ruft die möglichen Teilnahmen an Peanuts in diesem Monat, gruppiert nach Datum ab.
        /// </summary>
        public IDictionary<DateTime, IList<Core.Domain.Peanuts.Peanut>> AttendablePeanuts { get; private set; }

        /// <summary>
        ///     Ruft den Monat ab, der auf der Seite angezeigt wird.
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        ///     Ruft die Teilnahmen an Peanuts in diesem Monat, gruppiert nach Datum ab.
        /// </summary>
        public IDictionary<DateTime, IList<PeanutParticipation>> PeanutParticipations { get; }

        /// <summary>
        ///     Ruft das Jahr des Monats ab, dessen Peanuts angezeigt werden.
        /// </summary>
        public int Year { get; private set; }

        public IList<PeanutParticipation> GetPeanutParticipationsForDay(DateTime date) {
            if (!PeanutParticipations.ContainsKey(date.Date)) {
                return new List<PeanutParticipation>();
            } else {
                return PeanutParticipations[date.Date];
            }
        }
        public IList<Core.Domain.Peanuts.Peanut> GetAttendablePeanutForDay(DateTime date) {
            if (!AttendablePeanuts.ContainsKey(date.Date)) {
                return new List<Core.Domain.Peanuts.Peanut>();
            } else {
                return AttendablePeanuts[date.Date];
            }
        }
    }
}