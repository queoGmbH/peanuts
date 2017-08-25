using System;
using System.IO;
using System.Web;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils {
    /// <summary>
    ///     Hilfsklasse zur Erstellung von Dokumenten für Testfälle.
    /// </summary>
    public class DocumentCreator : EntityCreator {

        public IDocumentDao DocumentDao {
            get; set;
        }

        public DocumentCreator(string documentContentBasePath, string uploadedFileBasePath, IDocumentDao documentDao) {
            Require.NotNullOrWhiteSpace(documentContentBasePath, "documentContentBasePath");
            Require.NotNullOrWhiteSpace(uploadedFileBasePath, "uploadedFileBasePath");
            Require.NotNull(documentDao, "documentDao");

            DocumentDao = documentDao;

            DocumentContentBasePath = new DirectoryInfo(documentContentBasePath);
            // Directory erzeugen, wenn noch nicht vorhanden.
            if (!DocumentContentBasePath.Exists) {
                DocumentContentBasePath.Create();
                DocumentContentBasePath.Refresh();
            }


            UploadedFileBasePath = new DirectoryInfo(uploadedFileBasePath);
            // Directory erzeugen, wenn noch nicht vorhanden.
            if (!UploadedFileBasePath.Exists) {
                UploadedFileBasePath.Create();
                UploadedFileBasePath.Refresh();
            }
        }

        public DirectoryInfo DocumentContentBasePath { get; set; }

        public DirectoryInfo UploadedFileBasePath {
            get; set;
        }

        public Document Create(bool persist = true) {
            return Create(Guid.NewGuid() + ".jpg", persist);
        }

        public Document Create(string originalFileName, bool persist = true) {
            Require.NotNullOrWhiteSpace(originalFileName, "originalFileName");

            FileInfo originalFileInfo = new FileInfo(originalFileName);
            string tempFileName = Path.GetTempPath() + Guid.NewGuid() + originalFileInfo.Extension;
            FileInfo tempFileInfo = new FileInfo(tempFileName);

            using (tempFileInfo.Create()) {
            }
            tempFileInfo.MoveTo(DocumentContentBasePath.FullName + "\\" + tempFileInfo.Name);
            
            return Create(originalFileName, tempFileInfo.Name, MimeMapping.GetMimeMapping(tempFileInfo.Name), (int)tempFileInfo.Length, persist);
        }

        public Document Create(string originalFileName, string fileName, string contentType, int contentLength, bool persist = true) {

            Document document = new Document(originalFileName, Path.GetFileName(fileName), contentType, contentLength);
            if (persist) {
                DocumentDao.Save(document);
                DocumentDao.Flush();
            }

            return document;
        }

        public UploadedFile CreateUploadedFile() {
            string tempFileName = Path.GetTempFileName();
            FileInfo tempFileInfo = new FileInfo(tempFileName);

            using (tempFileInfo.Create()) {
            }
            tempFileInfo.MoveTo(UploadedFileBasePath.FullName + "\\" + tempFileInfo.Name);
            FileInfo uploadedFileInfo = new FileInfo(UploadedFileBasePath.FullName + "\\" + tempFileInfo.Name);
            return new UploadedFile(uploadedFileInfo);
        }
    }
}