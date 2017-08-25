using System.Diagnostics;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Stellt das Konto eines Nutzers in einer Gruppe dar.
    /// </summary>
    [DebuggerDisplay("{Membership.User.DisplayName} => {Membership.UserGroup.DisplayName}: {Balance}")]
    public class Account : Entity {
        private readonly UserGroupMembership _membership;

        private double _balance;

        public Account() {
        }

        public Account(UserGroupMembership membership) {
            _membership = membership;
        }

        public virtual double Balance {
            get { return _balance; }
        }

        public virtual UserGroupMembership Membership {
            get { return _membership; }
        }

        /// <summary>
        ///     Ändert die <see cref="Balance" /> um den angegebenen Wert.
        /// </summary>
        /// <param name="bookingAmount"></param>
        /// <returns></returns>
        public virtual double Book(double bookingAmount) {
            _balance += bookingAmount;
            return _balance;
        }

        public virtual string DisplayName {
            get {
                return string.Format("{0} ({1:c})", Membership.UserGroup.DisplayName, Balance);
            }
        }
    }
}