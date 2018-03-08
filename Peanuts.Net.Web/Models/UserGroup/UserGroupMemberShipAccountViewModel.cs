using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class UserGroupMemberShipAccountViewModel {
        public UserGroupMemberShipAccountViewModel(UserGroupMembership userGroupMembership, IPage<BookingEntry> bookings, UserGroupMembershipOptions userGroupMembershipOptions) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.NotNull(bookings, "bookings");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");

            UserGroupMembership = userGroupMembership;
            Bookings = bookings;
            UserGroupMembershipOptions = userGroupMembershipOptions;
        }

        /// <summary>
        /// Ruft die Mitgliedschaft ab, dessen zugehörigen Konto angezeigt wird.
        /// </summary>
        public UserGroupMembership UserGroupMembership { get; private set; }

        /// <summary>
        /// Ruft die anzuzeigenden Buchungen ab.
        /// </summary>
        public IPage<BookingEntry> Bookings { get; private set; }


        /// <summary>
        ///     Ruft die Optionen der Seite ab.
        /// </summary>
        public UserGroupMembershipOptions UserGroupMembershipOptions {
            get;
        }
    }
}