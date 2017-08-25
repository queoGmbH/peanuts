using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Display {
    /// <summary>
    /// Typ für das Panel der u.a. über Farben und anderen Styles für das Panel entscheidet.
    /// 
    /// Siehe http://getbootstrap.com/components/#panels
    /// </summary>
    public class PanelType {
        
        /// <summary>
        /// Ruft den Namen des Panel-Typen ab.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Ruft die CSS-Klasse(n) ab, die für diesen Panel-Typen benötigt werden.
        /// </summary>
        public string CssClass { get; private set; }

        private PanelType(string panelType, string classForType) {
            Require.NotNullOrWhiteSpace(panelType, "panelType");
            Require.NotNullOrWhiteSpace(classForType, "classForType");
            
            Type = panelType;
            CssClass = classForType;
            
        }

        /// <summary>
        /// Default Panel.
        /// 
        /// Das Panel erhält die Klasse panel-default
        /// </summary>
        public static PanelType Default {
            get {
                return new PanelType("Default", "panel panel-default");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static PanelType Primary
        {
            get
            {
                return new PanelType("Primary", "panel panel-primary");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static PanelType Success
        {
            get
            {
                return new PanelType("Success", "panel panel-success");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static PanelType Info
        {
            get
            {
                return new PanelType("Info", "panel panel-info");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static PanelType Warning
        {
            get
            {
                return new PanelType("Warning", "panel panel-warning");
            }
        }

        /// <summary>
        /// Primary Panel.
        /// 
        /// Das Panel erhält die Klasse panel-primary
        /// </summary>
        public static PanelType Danger
        {
            get
            {
                return new PanelType("Danger", "panel panel-danger");
            }
        }

        /// <summary>
        /// Erstellt einen eigenen Panel-Typen.
        /// </summary>
        /// <param name="panelType">Name des Panel-Typen</param>
        /// <param name="classForType">Klasse die ein Panel dieses Typen erhält.</param>
        /// <returns></returns>
        public static PanelType Custom(string panelType, string classForType) {
            return new PanelType(panelType, classForType);
        }

    }
}