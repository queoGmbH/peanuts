using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Home {
    public class DashboardNews {
        /// <inheritdoc />
        public DashboardNews(IList<PeanutParticipation> upcomingPeanutsAsProducer, IList<PeanutParticipation> upcomingPeanutsAsCreditor, IList<PeanutParticipation> todaysPeanutsAsParticipator, IList<PeanutParticipation> peanutInvitations, IList<Core.Domain.Peanuts.Peanut> upcomingPeanuts) {
            UpcomingPeanutsAsProducer = upcomingPeanutsAsProducer;
            UpcomingPeanutsAsCreditor = upcomingPeanutsAsCreditor;
            TodaysPeanutsAsParticipator = todaysPeanutsAsParticipator;
            PeanutInvitations = peanutInvitations;
            UpcomingPeanuts = upcomingPeanuts;
        }

        /// <summary>
        /// Ruft die Peanut-Teilnahmen ab, die heute stattfinden und für die der aktuelle Nutzer einen produzierenden Teilnahmetyp ausgewählt hat.
        /// </summary>
        public IList<PeanutParticipation> UpcomingPeanutsAsProducer { get; }

        /// <summary>
        /// Ruft die Peanut-Teilnahmen der nächsten Tage ab, bei denen sich der aktuelle Nutzer als Einkäufer eingetragen hat.
        /// </summary>
        public IList<PeanutParticipation> UpcomingPeanutsAsCreditor { get; }

        /// <summary>
        /// Ruft die Peanut-Teilnahmen des heutigen Tages ab, bei denen der Nutzer als normaler Teilnehmer zugesagt hat.
        /// </summary>
        public IList<PeanutParticipation> TodaysPeanutsAsParticipator { get; }

        /// <summary>
        /// Ruft die Peanuts ab, zu denen der Nutzer eingeladen wurde.
        /// </summary>
        public IList<PeanutParticipation> PeanutInvitations { get; }

        /// <summary>
        /// Ruft die kommenden Peanuts ab, an denen der Nutzer bisher nicht teilnimmt.
        /// </summary>
        public IList<Core.Domain.Peanuts.Peanut> UpcomingPeanuts { get; }
    }
}