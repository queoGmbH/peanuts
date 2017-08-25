namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {
    /// <summary>
    /// Beschreibt alle MenuItemViewModels, die eine Navigationsfunktionalität beinhalten.
    /// </summary>
    public interface INavigatingMenuItem {

        /// <summary>
        /// Ruft den Link ab, zu dem dieser Menüeintrag führt.
        /// Kann mit dem Link nicht navigiert werden, ist der Wert NULL.
        /// </summary>
        ActionLink Link {
            get;
        }

        /// <summary>
        /// Ruft die Zeichenfolge ab, die dem Element als Id zugewiesen wird.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Ruft den Text des Headers ab.
        /// </summary>
        string Text {
            get;
        }
    
    }
}