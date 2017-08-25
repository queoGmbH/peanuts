using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// ViewModel für eine Listenansicht mit Peanuts, über einen bestimmten Zeitraum.
    /// </summary>
    public class PeanutsListViewModel {
        public PeanutsListViewModel(DateTime from, DateTime to, IList<PeanutParticipation> peanutParticipations, IList<Core.Domain.Peanuts.Peanut> attendablePeanuts) {
            Require.NotNull(attendablePeanuts, "attendablePeanuts");
            Require.NotNull(peanutParticipations, "peanutParticipations");
            
            From = from;
            To = to;
            PeanutParticipations = peanutParticipations;
            AttendablePeanuts = attendablePeanuts;
        }

        /// <summary>
        /// Ruft das Datum ab, ab welchem die Peanuts angezeigt werden.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Ruft das Datum ab, bis zu welchem die Peanuts angezeigt werden.
        /// </summary>
        public DateTime To {
            get; set;
        }

        /// <summary>
        /// Ruft die Liste der Teilnahmen ab.
        /// </summary>
        public IList<PeanutParticipation> PeanutParticipations { get; private set; }

        /// <summary>
        /// Ruft die Liste der Peanuts ab, an denen der Nutzer nicht teilnimmt, aber teilnehmen könnte.
        /// </summary>
        public IList<Core.Domain.Peanuts.Peanut> AttendablePeanuts { get; private set; }

    }
}