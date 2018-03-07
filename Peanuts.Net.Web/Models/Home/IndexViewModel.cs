using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Home {
    public class IndexViewModel {
        public IndexViewModel(DashboardInfos dashboardInfos, User currentUser) {
            Require.NotNull(currentUser, "currentUser");
            Require.NotNull(dashboardInfos, "dashboardInfos");
            
            DashboardInfos = dashboardInfos;
            CurrentUser = currentUser;
        }

        /// <summary>
        /// Ruft gesammelte Informationen für die Anzeige auf dem Dashboard ab.
        /// Dort stehen zum Beispiel die Anzahlen, für die einzelnen Widgets auf dem Dashboard.
        /// </summary>
        public DashboardInfos DashboardInfos { get; }
        
        /// <summary>
        /// Liefert den Namen des Benutzers
        /// </summary>
        public User CurrentUser { get; private set; }
    }
}