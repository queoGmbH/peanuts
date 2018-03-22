using System;
using System.Diagnostics;
using System.Linq;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    /// <summary>
    ///     Bildet die Mitgliedschaft eines Nutzers in einer Gruppe ab.
    /// </summary>
    [DebuggerDisplay("{User.DisplayName} in {UserGroup.DisplayName}: {Account.Balance}")]
    public class UserGroupMembership : Entity {
        private readonly Account _account;
        private readonly DateTime _createdAt;
        private readonly User _createdBy;
        private readonly User _user;
        private readonly UserGroup _userGroup;
        private bool _autoAcceptBills;
        private DateTime? _changedAt;
        private User _changedBy;
        private UserGroupMembershipType _membershipType;

        public UserGroupMembership() {
        }

        public UserGroupMembership(UserGroup userGroup, User user, UserGroupMembershipType membershipType, EntityCreatedDto entityCreatedDto) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(user, "user");
            Require.NotNull(entityCreatedDto, "entityCreatedDto");

            _account = new Account(this);
            _userGroup = userGroup;
            _user = user;
            _membershipType = membershipType;
            _createdAt = entityCreatedDto.CreatedAt;
            _createdBy = entityCreatedDto.CreatedBy;
        }

        /// <summary>
        ///     Ruft alle Mitgliedschafts-Typen ab, von denen ein Mitglied einer Gruppe eines haben muss, um ein aktives Mitglied
        ///     der Gruppe zu sein.
        /// </summary>
        public static UserGroupMembershipType[] ActiveTypes {
            get { return new[] { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }; }
        }

        /// <summary>
        /// Ruft alle Mitgliedschafts-Typen ab, die ein Mitglied als in der Gruppe verfügbar markieren.
        /// Alle verfügbaren Nutzer können zum Beispiel explizit zu einem Peanut eingeladen werden oder eine Rechnung erhalten.
        /// </summary>
        public static UserGroupMembershipType[] AvailableTypes {
            get {
                return new[] { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member };
            }
        }

        /// <summary>
        /// Ruft alle Mitgliedschafts-Typen ab. 
        /// </summary>
        public static UserGroupMembershipType[] AllTypes {
            get {
                return Enum.GetValues(typeof(UserGroupMembershipType)).Cast<UserGroupMembershipType>().ToArray();
            }
        }

        /// <summary>
        ///     Ruft alle Mitgliedschafts-Typen ab, von denen ein Mitglied einer Gruppe eines haben muss, damit seine
        ///     Mitgliedschaft als inaktiv gilt.
        /// </summary>
        public static UserGroupMembershipType[] InactiveTypes {
            get { return new[] { UserGroupMembershipType.Inactive, UserGroupMembershipType.Quit, UserGroupMembershipType.Guest }; }
        }

        /// <summary>
        ///     Ruft alle Mitgliedschafts-Typen ab, von denen ein Mitglied einer Gruppe eines haben muss, damit seine
        ///     Mitgliedschaft als schwebend gilt.
        /// </summary>
        public static UserGroupMembershipType[] PendingTypes {
            get { return new[] { UserGroupMembershipType.Request, UserGroupMembershipType.Invited }; }
        }

        /// <summary>
        ///     Ruft das Konto des Nutzers in der Gruppe ab.
        /// </summary>
        public virtual Account Account {
            get { return _account; }
        }

        /// <summary>
        ///     Ruft ab, ob Rechnungen automatisch akzeptiert werden sollen.
        /// </summary>
        public virtual bool AutoAcceptBills {
            get { return _autoAcceptBills; }
        }

        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        public virtual string DisplayName {
            get { return string.Format("{0} - {1} ({2:c})", User.DisplayName, UserGroup.DisplayName, Account.Balance); }
        }

        /// <summary>
        ///     Ruft ab, ob es sich bei der Mitgliedschaft, um eine aktive Mitgliedschaft handelt.
        /// </summary>
        public virtual bool IsActiveMembership {
            get { return ActiveTypes.Contains(_membershipType); }
        }

        public virtual UserGroupMembershipType MembershipType {
            get { return _membershipType; }
        }

        public virtual User User {
            get { return _user; }
        }

        public virtual UserGroup UserGroup {
            get { return _userGroup; }
        }

        public virtual UserGroupMembershipDto GetDto() {
            return new UserGroupMembershipDto(_autoAcceptBills);
        }

        public virtual void Update(
            UserGroupMembershipType membershipType, UserGroupMembershipDto userGroupMembershipDto, EntityChangedDto entityChangedDto) {
            Require.NotNull(userGroupMembershipDto, "userGroupMembershipDto");
            Require.NotNull(entityChangedDto, "entityChangedDto");

            _membershipType = membershipType;
            Update(userGroupMembershipDto);
            Update(entityChangedDto);
        }

        public virtual void Update(
            UserGroupMembershipType membershipType, EntityChangedDto entityChangedDto) {
            Require.NotNull(entityChangedDto, "entityChangedDto");

            _membershipType = membershipType;
            Update(entityChangedDto);
        }

        public virtual void Update(UserGroupMembershipDto userGroupMembershipDto, EntityChangedDto entityChangedDto) {
            Require.NotNull(userGroupMembershipDto, "userGroupMembershipDto");
            Require.NotNull(entityChangedDto, "entityChangedDto");

            Update(userGroupMembershipDto);
            Update(entityChangedDto);
        }

        private void Update(UserGroupMembershipDto userGroupMembershipDto) {
            _autoAcceptBills = userGroupMembershipDto.AutoAcceptBills;
        }

        private void Update(EntityChangedDto entityChangedDto) {
            _changedAt = entityChangedDto.ChangedAt;
            _changedBy = entityChangedDto.ChangedBy;
        }
    }
}