using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    /// <summary>
    /// Model für ein Hidden-Input.
    /// </summary>
    public class HiddenInputModel  {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public HiddenInputModel(HtmlHelper htmlHelper, string propertyPath, object value) {
            Require.NotNull(htmlHelper, "htmlHelper");
            Require.NotNullOrWhiteSpace(propertyPath, "propertyPath");

            HtmlHelper = htmlHelper;
            PropertyPath = propertyPath;
            Value = string.Format("{0}",value);

            Id = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyPath);
            Name = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(PropertyPath);
        }

        /// <summary>
        /// Ruft den Html-Helper zum erzeugen des HTML-Markups ab.
        /// </summary>
        public HtmlHelper HtmlHelper { get; private set; }

        /// <summary>
        /// Ruft den für das "Form"-Element zu verwendenden Namen ab. Über diesen Namen wird das ModelBinding durchgeführt.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Ruft die für das "Form"-Element zu verwendende Id ab.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Ruft den Wert des Hidden-Fields ab.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Ruft den Pfad ab, der zum Property führt.
        /// </summary>
        public string PropertyPath { get; set; }

    }
}