using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    /// <summary>
    ///     Hilfsklasse für die Erzeugung einer Textbox.
    /// </summary>
    public abstract class InputModel : FormControlModel {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="propertyPath">Der Pfad zum Property</param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        public InputModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder) : base(htmlHelper, modelMetaData, propertyPath, label) {
            Placeholder = placeholder;
        }

        /// <summary>
        ///     Ruft den Input-Typen ab.
        ///     Siehe http://www.w3schools.com/tags/att_input_type.asp
        /// </summary>
        public abstract string InputType { get; }

        /// <summary>
        ///     Ruft den Platzhalter ab, der in der Textbox angezeigt werden soll, wenn kein Text vom Nutzer eingegeben wurde.
        /// </summary>
        public string Placeholder { get; private set; }
        
    }
}