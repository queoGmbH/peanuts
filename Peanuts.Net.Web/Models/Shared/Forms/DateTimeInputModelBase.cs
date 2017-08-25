using System.Globalization;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public abstract class DateTimeInputModelBase : InputModel {

        private ViewMode _minViewMode = ViewMode.Days;
        private ViewMode _defaultViewMode = ViewMode.Days;

        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="propertyPath">Der Pfad zum Property</param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        protected DateTimeInputModelBase(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder)
            : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {
        }

        /// <summary>
        /// Ruft das Format ab, in welchem das Datum oder die Zeit angezeigt bzw. eingegeben werden muss.
        /// </summary>
        public abstract string Format { get; }

        /// <summary>
        /// Ruft den als standard definierten Ansichtsmodus ab oder legt diesen fest.
        /// 
        /// Der Default ist Tagesansicht, was bedeutet, dass die Tagesauswahl angezeigt wird.
        /// </summary>
        public virtual ViewMode DefaultViewMode {
            get { return _defaultViewMode; }
            set { _defaultViewMode = value; }
        }

        /// <summary>
        /// Ruft den kleinsten möglichen Ansichtsmodus ab oder legt diesen fest.
        /// </summary>
        public virtual ViewMode MinViewMode {
            get { return _minViewMode; }
            set { _minViewMode = value; }
        }

        public string Locale {
            get { return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower(); }
        }

        /// <summary>
        /// Ansichts-Modus für den Datepicker.
        /// </summary>
        public enum ViewMode {
            /*Jahresauswahl*/
            Years,

            /*Monatsauswahl*/
            Month,

            /*Dekaden-/Jahrzehntauswahl*/
            Decades,

            /*Tagesauswahl*/
            Days
        }

        /// <summary>
        ///     Ruft den Input-Typen ab.
        ///     Siehe http://www.w3schools.com/tags/att_input_type.asp
        /// </summary>
        public override string InputType {
            get { return "text"; }
        }
    }
}