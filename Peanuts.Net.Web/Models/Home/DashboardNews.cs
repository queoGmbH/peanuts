using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Home {
    public class DashboardNews {
        /// <inheritdoc />
        public DashboardNews(User currentUser, bool showCurrentVersionNews, IList<PeanutParticipation> upcomingPeanutParticipations, IList<Core.Domain.Peanuts.Peanut> upcomingAttendablePeanuts) {
            Require.NotNull(currentUser, "currentUser");
            Require.NotNull(upcomingAttendablePeanuts, "upcomingAttendablePeanuts");
            Require.NotNull(upcomingPeanutParticipations, "upcomingPeanutParticipations");

            ShowCurrentVersionNews = showCurrentVersionNews;
            UpcomingPeanutParticipations = upcomingPeanutParticipations;
            UpcomingAttendablePeanuts = upcomingAttendablePeanuts;
            CurrentUser = currentUser;
        }

        /// <summary>
        /// Ruft ab, ob die Versionsinformationen der aktuellen Version angezeigt werden sollen.
        /// </summary>
        public bool ShowCurrentVersionNews {
            get; private set;
        }

        /// <summary>
        /// Ruft den aktuell angemeldeten Nutzer ab.
        /// </summary>
        public User CurrentUser { get; }

        /// <summary>
        /// Ruft die zugesagten Teilnahmen der nächsten Tage ab.
        /// </summary>
        public IList<PeanutParticipation> UpcomingPeanutParticipations { get; }

        /// <summary>
        /// Ruft die Peanuts der nächsten Tage ab, an denen der Nutzer bisher nicht teilnimmt.
        /// </summary>
        public IList<Core.Domain.Peanuts.Peanut> UpcomingAttendablePeanuts { get; }
    }
}