using System;
using System.Diagnostics;

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
        ///     Ruft das Konto des Nutzers in der Gruppe ab.
        /// </summary>
        public virtual Account Account {
            get { return _account; }
        }

        /// <summary>
        ///     Ruft alle Mitgliedschafts-Typen ab, von denen ein Mitglied einer Gruppe eines haben muss, um ein aktives Mitglied
        ///     der Gruppe zu sein.
        /// </summary>
        public static UserGroupMembershipType[] ActiveTypes {
            get { return new[] { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }; }
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
            get {
                return
                        _membershipType == UserGroupMembershipType.Administrator ||
                        _membershipType == UserGroupMembershipType.Member;
            }
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

        public virtual void Update(UserGroupMembershipType membershipType, EntityChangedDto entityChangedDto) {
            _membershipType = membershipType;

            Update(entityChangedDto);
        }

        private void Update(EntityChangedDto entityChangedDto) {
            _changedAt = entityChangedDto.ChangedAt;
            _changedBy = entityChangedDto.ChangedBy;
        }
    }
}