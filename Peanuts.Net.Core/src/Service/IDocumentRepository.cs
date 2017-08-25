using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public interface IDocumentRepository {
        /// <summary>
        ///     Erzeugt ein neues <see cref="Document" /> anhand des <see cref="UploadedFile" />
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        Document Create(UploadedFile uploadedFile);

        /// <summary>
        ///     Erzeugt aus einer Liste mit <see cref="UploadedFile" /> die Dokumente.
        /// </summary>
        /// <param name="uploadedFiles"></param>
        /// <returns></returns>
        IList<Document> Create(IList<UploadedFile> uploadedFiles);

        /// <summary>
        ///     Löscht das angegebene Document.
        /// </summary>
        /// <param name="document"></param>
        void Delete(Document document);

        /// <summary>
        ///     Liefert den vollen Dateinamen (inkl. Pfad) für die angegebene Datei im Temp-Ordner.
        ///     Eventuelle Pfadbestandteile im filename werden entfernt.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string GetFullFileName(string filename);

        /// <summary>
        ///     Liefert den vollen Dateinamen (inkl. Pfad) für die angegebene Datei im Temp-Ordner.
        ///     Eventuelle Pfadbestandteile im filename werden entfernt.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string GetFullTempFileName(string filename);
    }
}