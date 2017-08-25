using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Common.Logging;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement {
    /// <summary>
    ///     Klasse die Methoden für das Ausfüllen von Templates mit Platzhaltern bereitstellt.
    /// </summary>
    public class Templater {
        private const string PLACEHOLDER_CLOSING = "}";
        private const string PLACEHOLDER_FORMAT_SEPARATOR = ":";
        private const string PLACEHOLDER_OPENING = "{";
        private readonly ILog _logger = LogManager.GetLogger<Templater>();

        /// <summary>
        ///     Erzeugt einen neuen Templater, der die <see cref="CultureInfo.CurrentCulture">CurrentCulture</see> verwendet, um
        ///     Zeichenfolgen zu formatieren.
        /// </summary>
        public Templater() {
            Culture = CultureInfo.CurrentCulture;
        }

        /// <summary>
        ///     Erzeugt einen neuen Templater, mit abweichendem Default-Wert.
        /// </summary>
        /// <param name="defaultValue">
        ///     Der Wert durch den Platzhalter ersetzt werden sollen, deren Pfad im Model nicht gefunden
        ///     wird.
        /// </param>
        public Templater(string defaultValue)
            : this() {
            DefaultValue = defaultValue;
        }

        /// <summary>
        ///     Erzeugt einen neuen Templater.
        /// </summary>
        /// <param name="culture">
        ///     Legt die Kultur fest, in welcher die Zeichenfolgen ersetzt werden, wenn ein format für den
        ///     Platzhalter definiert ist.
        /// </param>
        /// <param name="defaultValue">
        ///     Der Wert durch den Platzhalter ersetzt werden sollen, deren Pfad im Model nicht gefunden
        ///     wird.
        /// </param>
        public Templater(CultureInfo culture, string defaultValue = null) {
            if (culture == null) {
                throw new ArgumentNullException("culture");
            }

            Culture = culture;
            DefaultValue = defaultValue;
        }

        /// <summary>
        ///     Ruft die Kultur ab, in welcher die Zeichenfolgen ersetzt werden, wenn ein format für den Platzhalter definiert ist.
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        ///     Ruft den Wert ab, der für einen Platzhalter eingefügt wird, wenn der Pfad im Model nicht gefunden wird.
        ///     Ist der Wert NULL werden die Platzhalter so gelassen wie sie im Template stehen.
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        ///     Füllt die Platzhalter im Template mit den Werten des Models.
        /// </summary>
        /// <param name="template">Das Template in welchem die Platzhalter ersetzt werden sollen.</param>
        /// <param name="model">Das Model mit den Werten für die Platzhalter</param>
        /// <returns></returns>
        public string FormatTemplate(string template, ModelMap model) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }

            StringBuilder regexStringBuilder = new StringBuilder();

            /*Platzhalter beginnt mit ...*/
            regexStringBuilder.Append("\\" + PLACEHOLDER_OPENING);

            /* Gruppe beginnen */
            regexStringBuilder.Append("(");

            /* Alle Zeichen die nicht dem Beginnzeichen und nicht dem Endzeichen entsprechen*/
            regexStringBuilder.Append("[^" + PLACEHOLDER_CLOSING + "]+");

            /* Gruppe beenden */
            regexStringBuilder.Append(")");

            /*Platzhalter endet mit ...*/
            regexStringBuilder.Append("\\" + PLACEHOLDER_CLOSING);

            if (_logger.IsDebugEnabled) {
                _logger.DebugFormat(
                    "In der Zeichenfolge [{0}] sollen alle Teilzeichenfolgen, die den RegEx [{1}] erfüllen, ersetzt werden.",
                    template,
                    regexStringBuilder.ToString());
            }

            /*Wie soll ersetzt werden.*/
            MatchEvaluator matchEvaluator = delegate(Match match) {
                try {
                    /*Pfad von Platzhalter-Öffner und -Schließer befreien.*/
                    string placeholder = match.Value.Replace(PLACEHOLDER_OPENING, "").Replace(PLACEHOLDER_CLOSING, "");
                    string[] strings = placeholder.Split(new[] { PLACEHOLDER_FORMAT_SEPARATOR }, 2, StringSplitOptions.None);
                    string format = "";

                    if (strings.Length >= 2) {
                        /*Wenn ein Format definiert ist, nutze dieses.*/
                        format = strings[1];
                    }

                    /*Platzhalter auslesen*/
                    string placeholderPath = strings[0];

                    if (_logger.IsDebugEnabled) {
                        _logger.DebugFormat("Der Platzhalter {0} soll im Format {1} ersetzt werden.", placeholderPath, format);
                    }

                    /*Wert anhand des Pfads aus dem Model auslesen.*/
                    PlaceholderModel placeholderModel = new PlaceholderModel(placeholderPath, model);
                    object valueByPath = placeholderModel.Value;

                    /*Wert zu string formatierenm, wenn nicht null */
                    if (valueByPath != null) {
                        if (!string.IsNullOrEmpty(format)) {
                            /*Wert formatieren, wenn Format definiert ist*/
                            return string.Format(Culture, "{0:" + format + "}", valueByPath);
                        }
                        return valueByPath.ToString();
                    }
                } catch (Exception ex) {
                    if (_logger.IsDebugEnabled) {
                        _logger.DebugFormat("Der Platzhalter {0} konnte nicht ersetzt werden und wird durch den Default-Value ersetzt.",
                            ex,
                            match.Value);
                    }
                }

                /*Wenn das ersetzen nicht funktiont, wird der Platzhalter so gelassen wie er ist (DefaultValue == null) oder der DefaultValue eingefügt (DefaultValue != null).*/
                if (DefaultValue == null) {
                    return match.Value;
                } else {
                    return DefaultValue;
                }
            };

            /*Nach Platzhaltern suchen.*/
            return Regex.Replace(template, regexStringBuilder.ToString(), matchEvaluator);
        }
    }

    internal class PlaceholderModel {
        private readonly string _placeholderName;
        private readonly string _placeholderPath;
        private readonly object _value;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public PlaceholderModel(string placeholder, ModelMap modelMap) {
            _placeholderName = GetPlaceholderName(placeholder);
            _placeholderPath = GetPlaceholderPath(placeholder);
            object placeholderObject = GetPlaceholderValue(modelMap, PlaceholderName);
            _value = GetValueFromModelByPath(PlaceholderPath, placeholderObject);
        }

        /// <summary>
        ///     Namensteil des Platzhalters.
        /// </summary>
        public string PlaceholderName {
            get { return _placeholderName; }
        }

        /// <summary>
        ///     Pfadanteil des Platzhalters.
        /// </summary>
        public string PlaceholderPath {
            get { return _placeholderPath; }
        }

        /// <summary>
        ///     Liefert den durch den Pfad definierten Wert.
        /// </summary>
        public object Value {
            get { return _value; }
        }

        private string GetPlaceholderName(string placeholder) {
            int indexOf = placeholder.IndexOf(".");
            if (indexOf <= 0) {
                return placeholder;
            }
            return placeholder.Substring(0, indexOf);
        }

        private string GetPlaceholderPath(string placeholder) {
            int indexOf = placeholder.IndexOf(".");
            if (indexOf <= 0) {
                return "";
            }
            return placeholder.Substring(indexOf + 1);
        }

        private object GetPlaceholderValue(ModelMap modelMap, string placeholderName) {
            if (modelMap.ContainsKey(placeholderName)) {
                return modelMap[PlaceholderName];
            }
            return null;
        }

        private object GetValueFromModelByPath(string placeholderPath, object model) {
            /*Wenn Model null, dann null liefern.*/
            if (model == null) {
                return null;
            }
            if (string.IsNullOrEmpty(placeholderPath)) {
                return model;
            }
            /*Platzhalter Pfad den Properties zuordnen*/
            string[] pathParts = placeholderPath.Split(new[] { '.' }, StringSplitOptions.None);

            object value = model.GetType().GetProperty(pathParts[0]).GetValue(model, null);

            if (pathParts.Length > 1) {
                /*Der Pfad hat noch mehr als 1 Stufe => im Model weiter abwärts gehen.*/
                return GetValueFromModelByPath(string.Join(".", pathParts.Skip(1)), value);
            }

            return value;
        }
    }
}