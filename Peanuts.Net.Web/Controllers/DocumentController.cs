using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Thumbnailing;

using NHibernate.Exceptions;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {

    [RoutePrefix("Document")]
    public class DocumentController : Controller {
        /// <summary>
        ///     Verarbeitet den asynchronen Upload einer Datei, und liefert Informationen über diese hochgeladene Datei als JSon.
        /// </summary>
        /// <param name="file"></param>
        /// <param name = "path" > Der Pfad für das Modelbinding.</param>
        /// <returns></returns>
        [Route("Async")]
        [HttpPost]
        public ActionResult UploadAsync(string path, UploadedFile file) {
            Require.NotNull(file, "file");

            ViewData.TemplateInfo.HtmlFieldPrefix = path;
            return PartialView("EditorTemplates/AsyncUploadedFile", file);
        }

       


        /// <summary>
        /// </summary>
        /// <param name="uploadedFile">Name der asynchron hochgeladenen Datei.</param>
        /// <returns></returns>
        [Route("Async/Preview/{uploadedFile}")]
        [HttpGet]
        public ActionResult UploadAsyncPreview(FileInfo uploadedFile) {
            Require.NotNull(uploadedFile, "uploadedFile");
            throw new NotImplementedException();
            //using (Stream filestream = uploadedFile.OpenRead()) {
            //    FileInfo thumbnailAsync = ImageThumbnailer.GetThumbnailAsync(new StreamWrapper(filestream, uploadedFile.Name), new ConvertOptions() {Height = 400, Width = 400}).Result;
            //    return File(thumbnailAsync.FullName, MimeMapping.GetMimeMapping(thumbnailAsync.Name), uploadedFile.Name);
            //}
        }

        public IDocumentRepository DocumentRepository { private get; set; }

        /// <summary>
        /// </summary>
        /// <param name="document">Das Bild dessen Vorschau angezeigt werden soll.</param>
        /// <returns></returns>
        [Route("{document:guid}/Thumbnail")]
        [HttpGet]
        public ActionResult Preview(Document document, int width = 400, int height = 400) {
            Require.NotNull(document, "document");
            throw new NotImplementedException();
            //using (Stream filestream = new FileInfo(DocumentRepository.GetFullFileName(document.FileName)).OpenRead()) {
            //    FileInfo thumbnailAsync = ImageThumbnailer.GetThumbnailAsync(new StreamWrapper(filestream, document.OriginalFileName), new ConvertOptions() { Height = height, Width = width }).Result;
            //    return File(thumbnailAsync.FullName, MimeMapping.GetMimeMapping(thumbnailAsync.Name), document.OriginalFileName);
            //}
        }

        /// <summary>
        /// </summary>
        /// <param name="document">Das Bild dessen Vorschau angezeigt werden soll.</param>
        /// <returns></returns>
        [Route("{document}/Image")]
        [HttpGet]
        public ActionResult Image(Document document) {
            Require.NotNull(document, "document");
            throw new NotImplementedException();
            //using (Stream filestream = new FileInfo(DocumentRepository.GetFullFileName(document.FileName)).OpenRead()) {
            //    FileInfo thumbnailAsync = ImageThumbnailer.GetThumbnailAsync(new StreamWrapper(filestream, document.OriginalFileName), new ConvertOptions() { Height = 1000, Width = 1000 }).Result;
            //    return File(thumbnailAsync.FullName, MimeMapping.GetMimeMapping(thumbnailAsync.Name), document.OriginalFileName);
            //}
        }
    }


    
}