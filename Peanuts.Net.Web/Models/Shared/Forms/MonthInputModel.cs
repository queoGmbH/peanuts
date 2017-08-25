using System.Globalization;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class MonthInputModel : DateTimeInputModelBase {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        public MonthInputModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder)
                : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {

            MinViewMode = ViewMode.Month;
        }

        /// <summary>
        ///     Ruft den Input-Typen ab.
        ///     Siehe http://www.w3schools.com/tags/att_input_type.asp
        /// </summary>
        public override string InputType {
            get { return "text"; }
        }

        public override object Value {
            get {
                return string.Format("{0:MM/yyyy}", base.Value);
            }
        }

        /// <summary>
        /// Ruft das Format ab, in welchem das Datum oder die Zeit angezeigt bzw. eingegeben werden muss.
        /// </summary>
        public override string Format {
            get {
                return "MM/YYYY";
                return CultureInfo.CurrentUICulture.DateTimeFormat.YearMonthPattern;
            }
        }
    }
}