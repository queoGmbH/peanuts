using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure {
    /// <summary>
    /// Die Ausnahme, die ausgelöst wird, wenn die Anzahl der Felder nicht mit der Anzahl der öffentlich, lesbaren Eigenschaften übereinstimmt.
    /// </summary>
    public class DifferentNumberOfFieldsAndPropertiesException:Exception {
        private readonly int _fieldsCount;
        private readonly int _propertiesCount;

        public DifferentNumberOfFieldsAndPropertiesException(int fieldsCount, int propertiesCount)
                : base("Dieser Test ist nur mit Typen ausführbar, bei denen die Anzahl der Felder ((Backing)-Fields) mit der Anzahl der öffentlich lesbaren Eigenschaften (public readonly Property) übereinstimmen.") {
            _fieldsCount = fieldsCount;
            _propertiesCount = propertiesCount;
        }

        /// <summary>
        /// Ruft die Anzahl der gefundenen Felder in der Klasse ab.
        /// </summary>
        public int FieldsCount {
            get { return _fieldsCount; }
        }

        /// <summary>
        /// Ruft die Anzahl der gefundenen öffentlichen, lesbaren Properties in der Klasse ab.
        /// </summary>
        public int PropertiesCount {
            get { return _propertiesCount; }
        }
    }
}