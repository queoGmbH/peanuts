using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NHibernate.Util;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    public class FileInfoModelBinder : IModelBinder {

        public FileInfoModelBinder(string uploadedFileBasePath) {
            UploadedFileBasePath = new DirectoryInfo(uploadedFileBasePath);
            // Directory erzeugen, wenn noch nicht vorhanden.
            if (!UploadedFileBasePath.Exists) {
                UploadedFileBasePath.Create();
                UploadedFileBasePath.Refresh();
            }
        }
        
        public DirectoryInfo UploadedFileBasePath {
            get; set;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            string modelName = bindingContext.ModelName;
            HttpFileCollectionBase httpFileCollectionBase = controllerContext.HttpContext.Request.Files;
            IList<HttpPostedFileBase> httpPostedFileBases = httpFileCollectionBase.GetMultiple(modelName).ToList();
            if (EnumerableExtensions.Any(httpPostedFileBases)) {
                /*Datei wurde direkt mit dem Post übertragen*/
                HttpPostedFileBase postedFile = httpPostedFileBases.First();

                /*Datei im UploadedFileBasePath ablegen*/
                string filename = UploadedFileBasePath.FullName + "/" + Guid.NewGuid() + "_" + postedFile.FileName;
                postedFile.SaveAs(filename);
                FileInfo fileInfo = new FileInfo(filename);
                
                /*Abgelegte Datei als Ergebnis liefern*/
                return fileInfo;
            } else {
                /* Vorheriger Asynchroner Upload => Datei im UploadedFileBasePath
                 * ValueProvider enthält den Namen der Datei im UploadedFileBasePath */
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
                string fileNameInUploadedFileBasePath = valueProviderResult.AttemptedValue;

                /*Datei aus dem Upload-Pfad holen*/
                FileInfo fileInfo = new FileInfo(UploadedFileBasePath + "/" + fileNameInUploadedFileBasePath);
                if (fileInfo.Exists) {
                    /*Wenn Datei existiert, liefere diese*/
                    return fileInfo;
                }
            }

            /*Es wurde keine Datei gefunden. ModelBinding liefert null*/
            return null;
        }

        
    }
}