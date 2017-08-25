namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {
    /// <summary>
    /// Basisklasse für alle ViewModels, die ein Menü-Item abbilden.
    /// </summary>
    public abstract class MenuItemViewModel {

        /// <summary>
        /// Ruft ab, ob der Menü-Eintrag derzeit aktiv ist.
        /// </summary>
        public virtual bool IsActive {
            get { return false; }
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob der Menü-Eintrag auswählbar ist oder nicht.
        /// </summary>
        public abstract bool IsEnabled { get; }
    }
}