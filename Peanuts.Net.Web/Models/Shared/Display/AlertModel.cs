using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Display {
    
    public class AlertModel {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.Object"/>-Klasse.
        /// </summary>
        /// <param name="alert">Die anzuzeigende Nachricht.</param>
        /// <param name="alertType">Welcher Art ist der Hinweis?</param>
        /// <param name="dismissable">Kann der Nutzer die Nachricht wegklicken?</param>
        public AlertModel(string alert, AlertType alertType, bool dismissable) {
            Require.NotNull(alertType, "alertType");

            Alert = alert;
            AlertType = alertType;
            Dismissable = dismissable;
        }

        /// <summary>
        /// Ruft den in der Alert-Box anzuzeigenden Text ab.
        /// </summary>
        public string Alert { get; private set; }

        /// <summary>
        /// Ruft die Art des Hinweises ab.
        /// </summary>
        public AlertType AlertType { get; private set; }

        /// <summary>
        /// Ruft ab, ob der Hinweis vom Nutzer "weggeklickt" werden kann.
        /// </summary>
        public bool Dismissable { get; private set; }

        public string GetDismissableCss() {
            if (Dismissable) {
                return "alert-dismissable";
            }

            return string.Empty;
        }
    }
}