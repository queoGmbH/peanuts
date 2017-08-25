using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure {
    /// <summary>
    /// Die Ausnahme, die ausgelöst wird, wenn keine Backing-Felder oder öffentlich, lesbaren Eigenschaften gefunden werden.
    /// </summary>
    public class MissingBackingPropertiesException:Exception {
        private readonly int _fieldsCount;
        private readonly int _propertiesCount;

        public MissingBackingPropertiesException(int fieldsCount, int propertiesCount)
                : base("Dieser Test ist nur mit Typen ausführbar, die mindestens ein Feld mit dazugehöriger Eigenschaft haben.") {

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