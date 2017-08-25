using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders {
    public class UserGroupMemberShipBuilder : Builder<UserGroupMembership> {
        private readonly IUserGroupDao _userGroupDao;
        private readonly DateTime _createdAt = new DateTime(2017, 01, 01, 15, 30, 00);
        private UserBuilder _createdBy;
        private User _user;
        private UserGroup _userGroup;

        public UserGroupMemberShipBuilder(IUserGroupDao userGroupDao) {
            _userGroupDao = userGroupDao;
        }

        public override UserGroupMembership Build() {
            if (_createdBy == null) {
                _createdBy = Create.A.User();
            }
            if (_userGroup == null) {
                _userGroup = Create.A.UserGroup();
            }
            if (_user == null) {
                _user = Create.A.User();
            }

            UserGroupMembership membership = new UserGroupMembership(_userGroup,
                _user,
                UserGroupMembershipType.Member,
                new EntityCreatedDto(_createdBy, _createdAt));
            _userGroupDao.Save(membership);
            _userGroupDao.Flush();

            return membership;
        }

        public UserGroupMemberShipBuilder ForUser(User user) {
            Require.NotNull(user, "user");
            _user = user;
            return this;
        }

        public UserGroupMemberShipBuilder InUserGroup(UserGroup userGroup) {
            Require.NotNull(userGroup, "userGroup");
            _userGroup = userGroup;
            return this;
        }
    }
}