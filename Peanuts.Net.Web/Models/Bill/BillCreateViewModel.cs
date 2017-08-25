using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Bill {
    /// <summary>
    ///     ViewModel für das Formular zu Erstellung einer Rechnung.
    /// </summary>
    public class BillCreateViewModel {

        /// <summary>
        /// Konstruktor, wenn die Rechnung unabhängig von einem Peanut erstellt wird.
        /// </summary>
        /// <param name="myUserGroupMemberships"></param>
        /// <param name="userGroupMembershipsInMyGroups"></param>
        public BillCreateViewModel(IList<UserGroupMembership> myUserGroupMemberships, IList<UserGroupMembership> userGroupMembershipsInMyGroups)
            : this(myUserGroupMemberships, userGroupMembershipsInMyGroups, new BillCreateCommand()) {
        }
        /// <summary>
        /// Konstruktor, wenn die Rechnung unabhängig von einem Peanut erstellt wird.
        /// </summary>
        /// <param name="myUserGroupMemberships"></param>
        /// <param name="userGroupMembershipsInMyGroups"></param>
        public BillCreateViewModel(IList<UserGroupMembership> myUserGroupMemberships, IList<UserGroupMembership> userGroupMembershipsInMyGroups, Core.Domain.Users.UserGroup userGroup)
            : this(myUserGroupMemberships, userGroupMembershipsInMyGroups, new BillCreateCommand(userGroup)) {
        }

        /// <summary>
        /// Konstruktor, wenn die Rechnung aus einem Peanut erstellt werden soll.
        /// </summary>
        /// <param name="myUserGroupMemberships"></param>
        /// <param name="userGroupMembershipsInMyGroups"></param>
        /// <param name="createdFromPeanut"></param>
        public BillCreateViewModel(IList<UserGroupMembership> myUserGroupMemberships, IList<UserGroupMembership> userGroupMembershipsInMyGroups,
            Core.Domain.Peanuts.Peanut createdFromPeanut)
            : this(myUserGroupMemberships, userGroupMembershipsInMyGroups, new BillCreateCommand(createdFromPeanut)) {
        }

        /// <summary>
        /// Konstruktor, wenn beim vorherigen Versuch eine Rechnung anzulegen etwas schief ging.
        /// </summary>
        /// <param name="myUserGroupMemberships"></param>
        /// <param name="userGroupMembershipsInMyGroups"></param>
        /// <param name="billCreateCommand"></param>
        public BillCreateViewModel(IList<UserGroupMembership> myUserGroupMemberships, IList<UserGroupMembership> userGroupMembershipsInMyGroups,
            BillCreateCommand billCreateCommand) {
            Require.NotNull(billCreateCommand, "billCreateCommand");
            Require.NotNull(myUserGroupMemberships, "myUserGroupMemberships");
            Require.NotNull(userGroupMembershipsInMyGroups, "userGroupMembershipsInMyGroups");

            MyUserGroupMemberships = myUserGroupMemberships;
            BillCreateCommand = billCreateCommand;
            UserGroupMembershipsInMyGroups = userGroupMembershipsInMyGroups;
        }

        /// <summary>
        ///     Ruft das Command zur Erstellung einer Rechnung ab.
        /// </summary>
        public BillCreateCommand BillCreateCommand { get; private set; }

        /// <summary>
        ///     Ruft meine Mitgliedschaften ab.
        ///     Nur dort kann ich als Kreditor/Gläubiger auftreten.
        /// </summary>
        public IList<UserGroupMembership> MyUserGroupMemberships { get; }

        /// <summary>
        ///     Ruft die Liste der möglichen Debitoren ab.
        /// </summary>
        public IList<UserGroupMembership> UserGroupMembershipsInMyGroups { get; set; }
    }
}