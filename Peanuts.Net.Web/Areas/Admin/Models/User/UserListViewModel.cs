using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User {
    /// <summary>
    ///     Model mit den Daten für die Anzeige der Nutzer als Liste im Backend.
    /// </summary>
    public class UserListViewModel {
        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="UserListViewModel" />
        /// </summary>
        /// <param name="users">Liste mit den Nutzern die in der Liste angezeigt werden.</param>
        /// <param name="searchTerm">Optionale Suchzeichenfolge, die als Filter für die angezeigten Nutzer verwendet wurde.</param>
        public UserListViewModel(IPage<Core.Domain.Users.User> users, string searchTerm, IList<Core.Domain.ProposedUsers.ProposedUser> proposedUsers) {
            Require.NotNull(users, "users");
            

            Users = users;
            SearchTerm = searchTerm;
            ProposedUsers = proposedUsers;
        }

        /// <summary>
        ///     Ruft die optionale Suchzeichenfolge ab, für welche die Nutzerliste angezeigt wird.
        /// </summary>
        public string SearchTerm { get; private set; }

        /// <summary>
        ///     Liefert oder setzt die Liste mit den Nutzern die angezeigt werden sollen.
        /// </summary>
        public IPage<Core.Domain.Users.User> Users { get; set; }

        public IList<Core.Domain.ProposedUsers.ProposedUser> ProposedUsers { get; set; }
        
    }
}