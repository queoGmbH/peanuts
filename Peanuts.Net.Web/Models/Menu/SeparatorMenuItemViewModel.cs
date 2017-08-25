namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {
    /// <summary>
    /// Bildet einen Separator in einem Menü ab.
    /// </summary>
    public class SeparatorMenuItemViewModel : MenuItemViewModel {
        private readonly bool _isEnabled;

        /// <summary>
        /// Erzeugt einen neuen Menu-Separator, der in der Regel als Quer-Strich angezeigt wird.
        /// </summary>
        /// <param name="isEnabled">Soll der Separator angezeigt werden.</param>
        public SeparatorMenuItemViewModel(bool isEnabled = true) {
            _isEnabled = isEnabled;
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob der Menü-Eintrag auswählbar ist oder nicht.
        /// </summary>
        public override bool IsEnabled {
            get { return _isEnabled; }
        }
    }
}