using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public abstract class FormControlModel {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        protected FormControlModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label = null) {
            Require.NotNull(htmlHelper, "htmlHelper");
            Require.NotNull(modelMetaData, "modelMetaData");

            HtmlHelper = htmlHelper;
            ModelMetaData = modelMetaData;
            PropertyPath = propertyPath;

            Label = GetLabel(modelMetaData, label);

            IsRequired = modelMetaData.IsRequired;
            
            Id = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyPath);
            Name = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(PropertyPath);
            Value = modelMetaData.Model;
        }

        public static string GetLabel(ModelMetadata modelMetaData, string label) {
            if (label != null) {
                /*Das Label wurde als Parameter übergeben => den Parameter verwenden*/
                return label;
            } else {
                if (!string.IsNullOrEmpty(modelMetaData.DisplayName)) {
                    /*Es wurde ein Display- bzw. ein DisplayNameAttribute am Property definiert*/
                    return modelMetaData.DisplayName;
                } else {
                    /*Label wird über die per Konvention definierte Resource versucht zu laden.*/
                    if (!string.IsNullOrWhiteSpace(modelMetaData.PropertyName)) {
                        return LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(modelMetaData.ContainerType, modelMetaData.PropertyName);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Ruft ab, ob das Feld ein Pflichtfeld ist.
        /// </summary>
        public bool IsRequired { get; private set; }

        /// <summary>
        /// Ruft den HTML-Helper ab.
        /// </summary>
        public HtmlHelper HtmlHelper { get; private set; }

        /// <summary>
        ///     Ruft die für das "Form"-Element zu verwendende Id ab.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        ///     Ruft den Text des Labels ab, das vor der Textbox in einem Label-Tag angezeigt wird.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        ///     Ruft den für das "Form"-Element zu verwendenden Namen ab. Über diesen Namen wird das ModelBinding durchgeführt.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Ruft den Pfad zum Property ab.
        /// </summary>
        public string PropertyName {
            get { return ModelMetaData.PropertyName; }
        }

        /// <summary>
        ///     Ruft den kompletten Pfad zum Property ab inklusive des <see cref="PropertyName" />.
        /// </summary>
        public string PropertyPath { get; set; }

        /// <summary>
        ///     Ruft den Wert ab, den das Control anzeigen soll.
        /// </summary>
        public virtual object Value { get; private set; }

        /// <summary>
        ///     Ruft die Meta-Daten für das Model ab.
        /// </summary>
        protected ModelMetadata ModelMetaData { get; set; }

        /// <summary>
        ///     Ruft die Validierungs-Attribute für unobtrusive JavaScript als Text ab.
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString GetValidationAttributes() {
            IDictionary<string, object> unobtrusiveValidationAttributes = HtmlHelper.GetUnobtrusiveValidationAttributes(Name, ModelMetaData);
            MvcHtmlString attributesString = new MvcHtmlString(string.Join(" ", unobtrusiveValidationAttributes.Select(attr => string.Format("{0}=\"{1}\"", attr.Key, HtmlHelper.Encode(ReplacePropertyNameWithLabel(attr.Value, PropertyName, Label))))));
            return attributesString;
        }

        private object ReplacePropertyNameWithLabel(object attributeValue, string propertyName, string replaceWith) {

            if (attributeValue == null) {
                return null;
            }
            string text = attributeValue as string;
            if (!string.IsNullOrWhiteSpace(text)) {
                return text.Replace("\"" + propertyName + "\"", string.Format("\"{0}\"", replaceWith));
            }

            return attributeValue;
        }
    }
}