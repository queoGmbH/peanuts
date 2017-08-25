using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.UserGroup {
    /// <summary>
    /// ViewModel für die Anzeige der Details eines <see cref="UserGroup"/>s.
    /// </summary>
    public class UserGroupShowViewModel {
        /// <summary>
        /// Erzeugt ein neues <see cref="UserGroupShowViewModel"/>
        /// </summary>
        /// <param name="userGroup"></param>
        /// <param name="members"></param>
        public UserGroupShowViewModel(Core.Domain.Users.UserGroup userGroup, IList<UserGroupMembership> members) {
            UserGroup = userGroup;
            Members = members;
        }

        public Core.Domain.Users.UserGroup UserGroup { get; set; }

        public IList<UserGroupMembership> Members { get; set; }


    }
}