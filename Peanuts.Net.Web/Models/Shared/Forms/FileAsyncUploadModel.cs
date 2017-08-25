using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class FileAsyncUploadModel : FileUploadModel {
        public FileAsyncUploadModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, string label, string placeholder, string uploadUrl) : base(htmlHelper, modelMetaData, propertyPath, label, placeholder) {
            UploadUrl = uploadUrl;
            
        }

        public string PreviewUrl { get; private set; }

        public string UploadUrl { get; private set; }
    }
}