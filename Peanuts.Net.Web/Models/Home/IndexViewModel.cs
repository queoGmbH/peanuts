using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Home {
    public class IndexViewModel {
        public IndexViewModel(DashboardInfos dashboardInfos, DashboardNews dashboardNews, User currentUser, bool showCurrentVersionNews) {
            Require.NotNull(dashboardInfos, "dashboardInfos");
            Require.NotNull(dashboardNews, "dashboardNews");

            DashboardInfos = dashboardInfos;
            DashboardNews = dashboardNews;
            CurrentUser = currentUser;
            ShowCurrentVersionNews = showCurrentVersionNews;
        }

        /// <summary>
        /// Ruft gesammelte Informationen für die Anzeige auf dem Dashboard ab.
        /// Dort stehen zum Beispiel die Anzahlen, für die einzelnen Widgets auf dem Dashboard.
        /// </summary>
        public DashboardInfos DashboardInfos { get; }

        /// <summary>
        /// Ruft die gesammelten Neuigkeiten ab, die auf der Startseite angezeigt werden.
        /// </summary>
        public DashboardNews DashboardNews { get; }

        /// <summary>
        /// Liefert den Namen des Benutzers
        /// </summary>
        public User CurrentUser { get; private set; }

        public bool ShowCurrentVersionNews { get; private set; }
    }
}