using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User;

using Microsoft.AspNet.Identity;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Manage {
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public UserShowViewModel User { get; set; }
    }
}