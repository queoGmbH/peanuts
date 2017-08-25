using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.UserGroup {

    /// <summary>
    /// ViewModel zur Anzeige der Übersicht über die <see cref="UserGroup"/>.
    /// </summary>
    public class UserGroupListViewModel {

        /// <summary>
        /// Erzeugt eine neue Instanz von <see cref="UserGroupListViewModel"/>.
        /// </summary>
        /// <param name="userGroups"></param>
        public UserGroupListViewModel(IPage<Core.Domain.Users.UserGroup> userGroups) {
            UserGroups = userGroups;
        }


        /// <summary>
        /// Liefert oder setzt die Liste mit den Maklergruppen für die Anzeige.
        /// </summary>
        public IPage<Core.Domain.Users.UserGroup> UserGroups { get; set; }
    }
}