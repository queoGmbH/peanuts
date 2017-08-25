using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {
    /// <summary>
    /// Klasse die einen Menüeintrag abbildet, der als DropDown-Menü angezeigt wird.
    /// </summary>
    public class DropDownMenuItemViewModel : MenuItemViewModel {
        private readonly bool _isActive;

        public DropDownMenuItemViewModel(string id, string text, bool isActive, IList<MenuItemViewModel> dropDownItems) {
            if (dropDownItems == null) {
                throw new ArgumentNullException("dropDownItems");
            }
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException("id");
            }
            if (string.IsNullOrWhiteSpace(text)) {
                throw new ArgumentNullException("text");
            }
            
            Id = id;
            Text = text;
            _isActive = isActive;
            DropDownItems = dropDownItems;
        }

        public IList<MenuItemViewModel> DropDownItems {
            get;
            private set;
        }

        /// <summary>
        /// Ruft ab, ob der Menü-Eintrag derzeit aktiv ist.
        /// </summary>
        public override bool IsActive {
            get {
                return _isActive;
            }
        }


        public string Id { get; set; }

        /// <summary>
        /// Ruft den Text ab, der im Menü angezeigt wird.
        /// </summary>
        public string Text {
            get;
            private set;
        }

        public override bool IsEnabled {
            get {
                /* Wenn irgendein Element im DropDown klickbar ist, kann auch das DropDown geöffnet werden. */
                return DropDownItems.Any(di => di.IsEnabled);
            }
        }
    }
}