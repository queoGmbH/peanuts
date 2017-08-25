using System;
using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {

    /// <summary>
    /// Model, dass den Inhalt des eines Menüs abbildet.
    /// </summary>
    public class MenuViewModel {
        private readonly IList<MenuItemViewModel> _menuItems = new List<MenuItemViewModel>();

        public MenuViewModel(IList<MenuItemViewModel> menuItemViewModels) {
            if (menuItemViewModels == null) {
                throw new ArgumentNullException("menuItemViewModels");
            }
            _menuItems = menuItemViewModels;
        }

        /// <summary>
        /// Ruft die MenuItems ab.
        /// </summary>
        public IList<MenuItemViewModel> MenuItems {
            get { return _menuItems; }
            
        }
    }
}