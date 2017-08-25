using System.IO;
using System.Web;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Documents {
    public class UploadedFile {
        public UploadedFile() {
        }

        public UploadedFile(FileInfo fileInfo) {
            Require.NotNull(fileInfo, "fileInfo");

            FileInfo = fileInfo;
            FileName = fileInfo.Name;
        }

        /// <summary>
        ///     Ruft die Größe der Datei in Byte ab.
        /// </summary>
        public long ContentLength {
            get {
                if (FileInfo == null || !FileInfo.Exists) {
                    return int.MinValue;
                }

                return FileInfo.Length;
            }
        }

        /// <summary>
        ///     Ruft den Typen der Datei ab.
        ///     https://de.wikipedia.org/wiki/Multipurpose_Internet_Mail_Extensions
        /// </summary>
        public string ContentType {
            get {
                if (FileInfo == null || !FileInfo.Exists) {
                    return "application/unknown";
                }

                return MimeMapping.GetMimeMapping(FileInfo.Name);
            }
        }

        /// <summary>
        ///     Ruft einen optionalen Copyright-Text ab oder legt diesen fest.
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        ///     Ruft einen optionalen Beschreibungstext ab oder legt diesen fest.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Ruft Informationen zur physisch existierenden Datei ab oder legt diese fest.
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        ///     Ruft den ursprünglichen Namen der Datei mit Erweiterung ab oder legt diesen fest.
        ///     Das Feld dient dazu sich den Namen der hochgeladenen Datei zu merken, da der Name der Datei auf dem Server geändert
        ///     werden kann.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Ruft einen optionalen Titel ab oder legt diesen fest.
        /// </summary>
        public string Title { get; set; }
    }
}