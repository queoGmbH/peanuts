using System;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    /// <summary>
    ///     Sucht die Expression inerhalb der Beschreibung eines Security-Attributs.
    /// </summary>
    public class AttributeExpressionParser {
        /// <summary>
        ///     Liefert einen Wert der angibt, ob es sich bei dem Wert um eine Expression handelt (handeln könnte),
        ///     also ob es eine öffnende und schliessenden geschweifte Klammer gibt.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsExpression(string value) {
            int openingBracket = value.IndexOf('{');
            int closingBracket = value.IndexOf('}');

            return openingBracket >= 0 && closingBracket >= 0 && closingBracket > openingBracket;
        }

        public static string Parse(string value) {
            Require.NotNullOrWhiteSpace(value, nameof(value));

            int startIndex = value.IndexOf('{');
            if (startIndex < 0) {
                throw new InvalidOperationException("Fehlerhafte Expression-Syntax: Öffnende Klammer nicht gefunden.");
            }
            int endIndex = value.IndexOf('}');
            if (endIndex == -1 || endIndex < startIndex) {
                throw new InvalidOperationException("Fehlerhafte Expression Syntax: Schliessende Klammer nicht gefunden oder vor öffnender Klammer.");
            }
            string expression = value.Substring(startIndex + 1, endIndex - startIndex - 1);

            return expression;
        }
    }
}