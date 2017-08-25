using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    /// <summary>
    /// Model für statischen Inhalt.
    /// </summary>
    public class StaticControlModel {

        /// <summary>
        /// Ruft den Label-Text des Controls ab.
        /// </summary>
        public string Label {
            get; private set;
        }

        /// <summary>
        /// Ruft den anzuzeigenden Wert des Controls ab.
        /// </summary>
        public object Value {
            get; private set;
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public StaticControlModel(HtmlHelper htmlHelper, string label, object value) {
            Label = label;
            Value = value;
        }
    }
}