using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders {
    public class UserBuilder : Builder<User> {

        private IUserDao _userDao;
        private User _createdBy;
        private DateTime _createdAt = new DateTime(2017, 01, 01, 15, 30, 00);
        private string _email = "max.mustermann@queo.de";
        private string _street = "Tharandter Str.";
        private string _streetNumber = "13";
        private string _postalCode = "01159";
        private string _city = "Dresden";
        private Country _country = Country.DE;
        private string _company = "";
        private string _url = "";
        private string _phone = "";
        private string _privatePhone = "";
        private string _mobile = "";
        private string _fax = "";
        private string _firstname = "Max";
        private string _lastname = "Mustermann";
        private string _internalInformation = "";
        private string _hobbies = "";
        private DateTime? _birthday = null;
        private IList<string> _roles = new List<string>();
        private bool _isEnabled = true;
        private string _agentId = "";
        private string _externalAgentId = "";
        private double? _commisionRate = null;
        private string _username = "";
        private string _passwordHash = "";
        private IList<Document> _documents = new List<Document>();

        public UserBuilder(IUserDao userDao) {
            _userDao = userDao;

            _username = "user_" + GetRandomString(5);
        }

        public override User Build() {
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(_createdBy, _createdAt);
            UserContactDto userContactDto = new UserContactDto(_email, _street, _streetNumber, _postalCode, _city, _country, _company, _url, _phone, _privatePhone, _mobile);
            UserDataDto userDataDto = new UserDataDto(_firstname, _lastname, _birthday, _username);
            UserPaymentDto userPaymentDto = new UserPaymentDto();
            UserNotificationOptionsDto notificationOptions = UserNotificationOptionsDto.AllOn;
            UserPermissionDto userPermissionDto = new UserPermissionDto(_roles, _isEnabled);
            
            User user = new User(_passwordHash, userContactDto, userDataDto, userPaymentDto, notificationOptions, userPermissionDto, _documents, entityCreatedDto);
            _userDao.Save(user);
            _userDao.Flush();
            
            return user;
        }

        public UserBuilder AndWithRole(string role) {
            if (!_roles.Contains(role)) {
                _roles.Add(role);
            }

            return this;
        }

        public UserBuilder AndWithRoles(params string[] roles) {
            foreach (string role in roles) {
                AndWithRole(role);
            }

            return this;
        }

        public UserBuilder WithRole(string role) {
            _roles.Clear();
            _roles.Add(role);

            return this;
        }

        public UserBuilder WithRoles(params string[] roles) {
            _roles.Clear();
            foreach (string role in roles) {
                _roles.Add(role);
            }

            return this;
        }

        public UserBuilder WithEmail(string email) {
            _email = email;
            return this;
        }
    }
}