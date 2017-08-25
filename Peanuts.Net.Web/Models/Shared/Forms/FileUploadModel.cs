using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class FileUploadModel : InputModel {
        /// <summary>
        /// </summary>
        /// <param name="modelMetaData"></param>
        /// <param name="htmlHelper"></param>
        /// <param name="label">Wenn ungleich null, wird das Label überschrieben, dass durch das MVC-Framework ermittelt wird.</param>
        /// <param name="placeholder"></param>
        public FileUploadModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder)
                : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {

            /*Überprüfen ob mehrere Dateien hochgeladen werden können. Das geht dann, wenn der Typ eine Liste ist.*/
            IsMultipleUpload = TypeHelper.IsListType(modelMetaData.ModelType);
        }

        /// <summary>
        ///     Ruft den Input-Typen ab.
        ///     Siehe http://www.w3schools.com/tags/att_input_type.asp
        /// </summary>
        public override string InputType {
            get { return "file"; }
        }

        /// <summary>
        /// Ruft ab, ob mehrere Dateien hochgeladen werden können.
        /// </summary>
        public bool IsMultipleUpload {
            get; private set; }

        /// <summary>
        /// Liefert das Attribute für einen evtl. Mehrfach-Upload.
        /// </summary>
        /// <returns></returns>
        public string GetMultipleAttribute() {
            if (IsMultipleUpload) {
                return "multiple";

            }

            return "";
        }
    }
}