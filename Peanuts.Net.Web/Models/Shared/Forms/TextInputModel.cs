using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class TextInputModel : InputModel {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="propertyPath"></param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        /// <param name="formatString">
        ///     Zeichenfolge zur <see cref="string.Format(string,object)">Formatierung</see> des
        ///     anzuzeigenden Wertes oder null wenn der Wert nicht formatiert werden soll.
        /// </param>
        public TextInputModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder, string formatString = null)
                : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {
            FormatString = formatString;
        }

        /// <summary>
        ///     Ruft die Formatierungszeichenfolge ab, mit welcher der anzuzeigende Text formatiert werden soll.
        ///     Ist keine Formatierung vorgesehen, dann null.
        /// </summary>
        public string FormatString { get; private set; }

        /// <summary>
        ///     Ruft den Input-Typen ab.
        ///     Siehe http://www.w3schools.com/tags/att_input_type.asp
        /// </summary>
        public override string InputType {
            get { return "text"; }
        }

        /// <summary>
        ///     Ruft den Wert ab, den das Control anzeigen soll.
        /// </summary>
        public override object Value {
            get {
                if (!string.IsNullOrWhiteSpace(FormatString)) {
                    return string.Format(FormatString, base.Value);
                }

                return base.Value;
            }
        }
    }
}