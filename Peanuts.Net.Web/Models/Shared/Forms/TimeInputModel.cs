using System.Globalization;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class TimeInputModel : DateTimeInputModelBase {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="propertyPath"></param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        public TimeInputModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder)
                : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {
        }

        /// <summary>
        /// Ruft das Format ab, in welchem das Datum oder die Zeit angezeigt bzw. eingegeben werden muss.
        /// </summary>
        public override string Format {
            get {
                return "HH:mm";
                return CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;
            }
        }
    }
}