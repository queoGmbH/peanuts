using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    [DtoFor(typeof(UserGroupMembership))]
    public class UserGroupMembershipDto {
        /// <inheritdoc />
        public UserGroupMembershipDto() {
        }

        /// <inheritdoc />
        public UserGroupMembershipDto(bool autoAcceptBills) {
            AutoAcceptBills = autoAcceptBills;
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob Rechnungen automatisch akzeptiert werden sollen.
        /// </summary>
        public bool AutoAcceptBills { get; set; }

        protected bool Equals(UserGroupMembershipDto other) {
            return AutoAcceptBills == other.AutoAcceptBills;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserGroupMembershipDto)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return AutoAcceptBills.GetHashCode();
        }
    }
}