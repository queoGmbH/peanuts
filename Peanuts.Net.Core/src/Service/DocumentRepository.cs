using System.Collections.Generic;
using System.IO;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public class DocumentRepository : IDocumentRepository {
        public DocumentRepository() {
        }

        public DocumentRepository(string documentContentBasePath, IDocumentDao documentDao, string uploadedFileBasePath) {
            Require.NotNullOrWhiteSpace(documentContentBasePath, "documentContentBasePath");
            Require.NotNullOrWhiteSpace(uploadedFileBasePath, "uploadedFileBasePath");

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

            DocumentDao = documentDao;
        }

        public DirectoryInfo DocumentContentBasePath { get; set; }

        public IDocumentDao DocumentDao { get; set; }

        public DirectoryInfo UploadedFileBasePath { get; set; }

        [Transaction]
        public Document Create(UploadedFile uploadedFile) {
            Require.NotNull(uploadedFile, nameof(uploadedFile));

            Document document = new Document(uploadedFile);
            CreateContent(uploadedFile);
            document = DocumentDao.Save(document);
            return document;
        }

        /// <summary>
        ///     Erzeugt aus einer Liste mit <see cref="UploadedFile" /> die Dokumente.
        /// </summary>
        /// <param name="uploadedFiles"></param>
        /// <returns></returns>
        [Transaction]
        public IList<Document> Create(IList<UploadedFile> uploadedFiles) {
            Require.NotNull(uploadedFiles, nameof(uploadedFiles));
            List<Document> documents = new List<Document>();
            foreach (UploadedFile uploadedFile in uploadedFiles) {
                if (uploadedFile != null) {
                    documents.Add(Create(uploadedFile));
                }
            }
            return documents;
        }

        /// <summary>
        ///     Löscht das angegebene Document.
        /// </summary>
        /// <param name="document"></param>
        public void Delete(Document document) {
            Require.NotNull(document, nameof(document));

            DeleteContent(document);
            DocumentDao.Delete(document);
        }

        public string GetFullFileName(string filename) {
            Require.NotNullOrWhiteSpace(filename, "filename");

            return Path.Combine(DocumentContentBasePath.FullName, Path.GetFileName(filename));
        }

        public string GetFullTempFileName(string filename) {
            Require.NotNullOrWhiteSpace(filename, "filename");

            return Path.Combine(UploadedFileBasePath.FullName, Path.GetFileName(filename));
        }

        private void CreateContent(UploadedFile uploadedFile) {
            Require.NotNull(uploadedFile, "uploadedFile");

            string fullFilename = GetFullFileName(uploadedFile.FileInfo.Name);
            uploadedFile.FileInfo.CopyTo(fullFilename);
        }

        private void DeleteContent(Document document) {
            string fullFileName = GetFullFileName(document.FileName);
            FileInfo fileInfo = new FileInfo(fullFileName);
            if (fileInfo.Exists) {
                fileInfo.Delete();
            }
        }
    }

    public class FileDocumentContentRepository : IDocumentContentRepository {
        public DirectoryInfo DocumentContentBasePath { get; set; }

        public DirectoryInfo UploadedFileBasePath { get; set; }

        public string CreateContent(UploadedFile uploadedFile) {
            return null;
        }
    }

    public class TemporaryDocumentContentRepository : IDocumentContentRepository {
        public string CreateContent(UploadedFile uploadedFile) {
            return Path.GetFileName(uploadedFile.FileInfo.Name);
        }
    }

    public interface IDocumentContentRepository {
        string CreateContent(UploadedFile uploadedFile);
    }
}