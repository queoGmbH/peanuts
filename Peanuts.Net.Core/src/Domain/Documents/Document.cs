using System.Diagnostics;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Documents {
    /// <summary>
    ///     Bildet eine Datei innerhalb der Anwendung ab.
    /// </summary>
    [DebuggerDisplay("{Id} => {FileName}")]
    public class Document : Entity {
        private readonly long _contentLength;
        private readonly string _contentType;
        private readonly string _fileName;
        private readonly string _originalFileName;

        /// <summary>
        ///     Ctor. für Hibernate.
        /// </summary>
        public Document() {
        }

        public Document(string originalFileName, string fileName, string contentType, int contentLength) {
            Require.NotNullOrWhiteSpace(originalFileName, "originalFileName");
            Require.NotNullOrWhiteSpace(fileName, "fileName");
            Require.NotNullOrWhiteSpace(contentType, "contentType");
            Require.Ge(contentLength, 0, "contentLength");

            _contentLength = contentLength;
            _contentType = contentType;
            _fileName = fileName;
            _originalFileName = originalFileName;
        }

        /// <summary>
        ///     Erzeugt eine neue <see cref="Document" />.
        /// </summary>
        /// <param name="uploadedFile"></param>
        public Document(UploadedFile uploadedFile) {
            Require.NotNull(uploadedFile, nameof(uploadedFile));

            _contentLength = uploadedFile.ContentLength;
            _contentType = uploadedFile.ContentType;
            _originalFileName = uploadedFile.FileName;
            _fileName = uploadedFile.FileInfo.Name;
        }

        /// <summary>
        ///     Liefert die Anzahl der Bytes der Datei.
        /// </summary>
        public virtual long ContentLength {
            get { return _contentLength; }
        }

        /// <summary>
        ///     Liefert den ermittelten ContentType.
        /// </summary>
        public virtual string ContentType {
            get { return _contentType; }
        }

        /// <summary>
        ///     Liefert den Dateinamen.
        /// </summary>
        /// <remarks>
        ///     Der Dateiname beinhaltet einen relativen Pfad innerhalb der Ablage.
        /// </remarks>
        public virtual string FileName {
            get { return _fileName; }
        }

        /// <summary>
        ///     Liefert den originalen Dateinamen.
        /// </summary>
        public virtual string OriginalFileName {
            get { return _originalFileName; }
        }
    }
}