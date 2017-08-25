using System.Collections.Generic;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class DeleteDocumentsModel {
        public DeleteDocumentsModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, IList<Document> documents) {
            Require.NotNull(htmlHelper, "htmlHelper");
            Require.NotNull(modelMetaData, "modelMetaData");

            HtmlHelper = htmlHelper;
            ModelMetaData = modelMetaData;
            PropertyPath = propertyPath;
            Documents = documents;
        }

        public string PropertyPath { get; set; }

        public IList<Document> Documents { get; set; }

        public ModelMetadata ModelMetaData { get; set; }

        public HtmlHelper HtmlHelper { get; set; }
    }
}