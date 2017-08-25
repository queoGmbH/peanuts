using System;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {
    /// <summary>
    /// Bildet einen simplen Menüeintrag ab, der geklickt werden kann und zu irgendeiner URL navigiert.
    /// </summary>
    public class SimpleMenuItemViewModel : MenuItemViewModel, INavigatingMenuItem {
        
        private readonly bool _isActive;
        private readonly bool _isEnabled;

        public SimpleMenuItemViewModel(string id, ActionLink link, bool isActive, string iconClass, bool isEnabled = true) {
            if (link == null) {
                throw new ArgumentNullException("link");
            }
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException("id");
            }
            
            Id = id;
            Link = link;
            IconClass = iconClass;
            _isActive = isActive;
            _isEnabled = isEnabled;
        }

        /// <summary>
        /// Ruft ab, ob der Menü-Eintrag derzeit aktiv ist.
        /// </summary>
        public override bool IsActive {
            get { return _isActive; } }

        public override bool IsEnabled {
            get { return _isEnabled; } 
        }

        public string Id { get; private set; }

        /// <summary>
        /// Ruft den Text des Headers ab.
        /// </summary>
        public string Text {
            get { return Link.Text; }
        }
        
        /// <summary>
        /// Ruft den Link ab, zu dem dieser Menüeintrag führt.
        /// Kann mit dem Link nicht navigiert werden, ist der Wert NULL.
        /// </summary>
        public ActionLink Link { get; private set; }

        public string IconClass { get; private set; }
    }
}