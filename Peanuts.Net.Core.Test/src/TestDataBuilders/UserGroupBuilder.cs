using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders {
    public class UserGroupBuilder : Builder<UserGroup> {
        private readonly IUserGroupDao _userGroupDao;

        private DateTime _createdAt = new DateTime(2017, 01, 01, 15, 30, 00);
        private User _createdBy;

        private string _additionalInformation = "";
        private string _name;
        private int _accepteedBalance = -10;
        static Random _random = new Random();

        public UserGroupBuilder(IUserGroupDao userGroupDao) {
            _userGroupDao = userGroupDao;
            _name = "Gruppe " + _random.Next(1, 9999);
        }

        public override UserGroup Build() {

            if (_createdBy == null) {
                _createdBy = Create.A.User();
            }

            UserGroup userGroup = new UserGroup(new UserGroupDto(_additionalInformation, _name, _accepteedBalance), new EntityCreatedDto(_createdBy, _createdAt));
            _userGroupDao.Save(userGroup);
            _userGroupDao.Flush();
            return userGroup;
        }
    }
}