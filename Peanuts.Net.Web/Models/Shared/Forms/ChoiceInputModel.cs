using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public abstract class ChoiceInputModel : FormControlModel {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        protected ChoiceInputModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label) : base(htmlHelper, modelMetaData, propertyPath, label) {
        }

        /// <summary>
        ///     Ruft den Input-Typen ab.
        ///     Siehe http://www.w3schools.com/tags/att_input_type.asp
        /// </summary>
        public abstract string InputType { get; }

        public string GetChecked() {
            if (ModelMetaData.Model as bool? == true) {
                return "checked=\"checked\"";
            }
            return "";
        }
    }
}