using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure {
    /// <summary>
    /// Excpetion die geworfen wird, wenn von einem Typ ein parameterloser Konstruktor erwartet wird, dieser aber nicht vorhanden.
    /// </summary>
    public class MissingParameterlessConstructorException:Exception {
        private readonly Type _typeWithoutParameterlessConstructor;

        /// <summary>
        /// Erzeugt eine neue Instanz der MissingParameterlessConstructorException-Klasse.
        /// </summary>
        /// <param name="type">Der Typ der nicht über einen parameterlosen Konstruktor verfügt.</param>
        public MissingParameterlessConstructorException(Type type)
                : base("Vom Typ (" + type + ") wurde ein parameterloser Konstruktor erwartet.") {
            _typeWithoutParameterlessConstructor = type;
        }

        /// <summary>
        /// Ruft den Typ ab, der nicht über einen parameterlosen Konstruktor verfügt.
        /// </summary>
        public Type TypeWithoutParameterlessConstructor {
            get { return _typeWithoutParameterlessConstructor; }
        }
    }
}