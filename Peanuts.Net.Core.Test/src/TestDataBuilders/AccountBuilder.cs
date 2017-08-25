using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders {
    public class AccountBuilder : Builder<Account> {
        private readonly IUserGroupDao _userGroupDao;
        private UserGroupMembership _userGroupMembership = null;

        public AccountBuilder(IUserGroupDao userGroupDao) {
            _userGroupDao = userGroupDao;
        }

        public AccountBuilder InGroup(UserGroupMembership userGroupMembership) {
            _userGroupMembership = userGroupMembership;
            return this;
        }

        public override Account Build() {

            Account account;
            if (_userGroupMembership == null) {
                _userGroupMembership = Create.A.UserGroupMembership();
                account = _userGroupMembership.Account;
            } else {
                account = new Account(_userGroupMembership);
                _userGroupDao.Save(_userGroupMembership.UserGroup);
            }

            return account;
        }
    }
}