namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.Home {
    /// <summary>
    ///     ViewModel für die Startseite des Verwaltungsbereichs.
    /// </summary>
    public class HomeAdministrationViewModel {
        public HomeAdministrationViewModel(long userCount, long userGroupCount) {
            UserCount = userCount;
            UserGroupCount = userGroupCount;
        }

        /// <summary>
        ///     Ruft die Anzahl der aktuell im System erfassten, aktiven Nutzer ab.
        /// </summary>
        public long UserCount { get; private set; }

        /// <summary>
        ///     Ruft die Anzahl der aktuell im System erfassten Nutzergruppen ab.
        /// </summary>
        public long UserGroupCount { get; private set; }
    }
}