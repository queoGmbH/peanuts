using System.IO;
using System.Text;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class TemporaryTextWriter : StringWriter {
        private readonly Encoding _encoding;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.IO.TextWriter"/>-Klasse.
        /// </summary>
        public TemporaryTextWriter(Encoding encoding) {
            Require.NotNull(encoding, "encoding");
            

            _encoding = encoding;
        }
        
        /// <summary>
        /// Gibt beim Überschreiben in einer abgeleiteten Klasse die Zeichencodierung zurück, in der die Ausgabe geschrieben ist.
        /// </summary>
        /// <returns>
        /// Die Channelverschlüsselung, in der die Ausgabe geschrieben wird.
        /// </returns>
        public override Encoding Encoding {
            get { return _encoding; }
        }

    }
}