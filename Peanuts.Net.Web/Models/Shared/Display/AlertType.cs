using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Display {
    /// <summary>
    /// Typ für den Alert der u.a. über Farben und anderen Styles für die Hinweisbox entscheidet.
    /// 
    /// Siehe http://getbootstrap.com/components/#alerts
    /// </summary>
    public class AlertType {
        
        /// <summary>
        /// Ruft den Namen des Alert-Typen ab.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Ruft die CSS-Klasse(n) ab, die für diesen Alert-Typen benötigt werden.
        /// </summary>
        public string CssClass { get; private set; }

        private AlertType(string alertType, string classForType)
        {
            Require.NotNullOrWhiteSpace(alertType, "alertType");
            Require.NotNullOrWhiteSpace(classForType, "classForType");
            
            Type = alertType;
            CssClass = classForType;
            
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static AlertType Success
        {
            get
            {
                return new AlertType("Success", "alert-success");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static AlertType Info
        {
            get
            {
                return new AlertType("Info", "alert-info");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static AlertType Warning
        {
            get
            {
                return new AlertType("Warning", "alert-warning");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static AlertType Danger
        {
            get
            {
                return new AlertType("Danger", "alert-danger");
            }
        }

        /// <summary>
        /// Erstellt einen eigenen Alert-Typen.
        /// </summary>
        /// <param name="panelType">Name des Alert-Typen</param>
        /// <param name="classForType">Klasse die ein Alert dieses Typs erhält.</param>
        /// <returns></returns>
        public static AlertType Custom(string panelType, string classForType)
        {
            return new AlertType(panelType, classForType);
        }

    }
}