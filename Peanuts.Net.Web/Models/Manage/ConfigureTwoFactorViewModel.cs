using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Manage {
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}