using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User {
    /// <summary>
    /// ViewModel für die Ansicht zum Bestätigen des Löschens eines Nutzers.
    /// </summary>
    public class UserDeleteViewModel {

        public UserDeleteViewModel(Core.Domain.Users.User userToDelete) {
            Require.NotNull(userToDelete, "userToDelete");

            UserToDelete = userToDelete;
        }

        /// <summary>
        /// Ruft den zu löschenden Nutzer ab.
        /// </summary>
        public Core.Domain.Users.User UserToDelete { get; private set; }

    }
}