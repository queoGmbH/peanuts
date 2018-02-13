using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    ///     ViewModel für die Anzeige der Peanuts in einer Gruppe.
    /// </summary>
    public class UserGroupPeanutsViewModel {
        public UserGroupPeanutsViewModel(
            Core.Domain.Users.UserGroup userGroup, UserGroupMembership currentUsersMembershipInGroup, IPage<Core.Domain.Peanuts.Peanut> peanuts, UserGroupMembershipOptions userGroupMembershipOptions) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");
            Require.NotNull(peanuts, "peanuts");

            UserGroup = userGroup;
            CurrentUsersMembershipInGroup = currentUsersMembershipInGroup;
            UserGroupMembershipOptions = userGroupMembershipOptions;
            Peanuts = peanuts;
        }

        /// <summary>
        ///     Liefert die Liste der Peanuts
        /// </summary>
        public IPage<Core.Domain.Peanuts.Peanut> Peanuts { get; set; }

        /// <summary>
        ///     Ruft die Nutzergruppe ab, deren Peanuts angezeigt werden..
        /// </summary>
        public Core.Domain.Users.UserGroup UserGroup { get; }

        /// <summary>
        /// Ruft die Mitgliedschaft des aktuellen Nutzers in der Gruppe ab.
        /// Ist der aktuelle Nutzer kein Mitglied in der Gruppe wird null geliefert.
        /// </summary>
        public UserGroupMembership CurrentUsersMembershipInGroup { get; }

        /// <summary>
        ///     Ruft die Optionen der Seite ab.
        /// </summary>
        public UserGroupMembershipOptions UserGroupMembershipOptions { get; }
    }
}