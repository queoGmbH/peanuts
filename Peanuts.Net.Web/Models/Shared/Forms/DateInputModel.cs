using System;
using System.Globalization;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class DateInputModel : DateTimeInputModelBase {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="propertyPath"></param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        public DateInputModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder)
            : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {
        }

        /// <summary>
        /// Ruft den Wert ab, den das Control anzeigen soll.
        /// </summary>
        public override object Value {
            get {
                DateTime? dateValue = base.Value as DateTime?;
                if (dateValue.HasValue) {
                    if (dateValue.Value.Year < 1800) {
                        dateValue = DateTime.Today;
                    }

                    return dateValue.Value.ToShortDateString();
                }

                return null;
            }
        }

        /// <summary>
        /// Ruft das Format ab, in welchem das Datum oder die Zeit angezeigt bzw. eingegeben werden muss.
        /// </summary>
        public override string Format {
            get {
                return "DD.MM.YYYY";
                return CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            }
        }
    }
}