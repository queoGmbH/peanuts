using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class TextAreaModel : FormControlModel
    {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="resizeable">Kann die größe der TextArea durch den Nutzer angepasst werden.</param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        public TextAreaModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, bool resizeable, string label, string placeholder) : base(htmlHelper, modelMetaData, propertyPath, label) {
            Resizeable = resizeable;
            Placeholder = placeholder;
        }

        /// <summary>
        /// Ruft ab, ob der Nutzer die größe der TextArea anpassen kann.
        /// </summary>
        public bool Resizeable { get; private set; }

        /// <summary>
        ///     Ruft den Platzhalter ab, der in der TextArea angezeigt werden soll, wenn kein Text vom Nutzer eingegeben wurde.
        /// </summary>
        public string Placeholder { get; private set; }


        /// <summary>
        /// Ruft die Klasse ab, die verwendet wird, um das Resizing für die Textarea zu aktivieren oder zu deaktivieren.
        /// Je nach Wert der Eigenschaft <see cref="Resizeable"/>
        /// </summary>
        /// <returns></returns>
        public string GetResizeable() {
            return Resizeable ? "resize" : "";
        }
    }
}