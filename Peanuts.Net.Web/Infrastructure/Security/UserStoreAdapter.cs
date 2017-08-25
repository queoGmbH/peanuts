using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Service;

using Common.Logging;

using Microsoft.AspNet.Identity;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Implementierung und Adaptierung des UserStores. Der UserStore stellt Methoden für den UserManager bereit, die den
    ///     UserService benutzen, um persistierte Nutzerdaten zu holen.
    /// </summary>
    public class UserStoreAdapter : IUserStore<SecurityUser>,
            IUserPasswordStore<SecurityUser>,
            IUserEmailStore<SecurityUser>,
            IUserLockoutStore<SecurityUser, string>,
            IUserTwoFactorStore<SecurityUser, string>,
            IUserLoginStore<SecurityUser> {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UserStoreAdapter));

        /// <summary>
        ///     Liefert den UserService.
        /// </summary>
        public IUserService UserService { get; set; }

        public Task AddLoginAsync(SecurityUser user, UserLoginInfo login) {
            throw new NotImplementedException();
        }

        public Task CreateAsync(SecurityUser user) {
            string baseLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            User user1 = UserService.Register(user.UserName,
                user.PasswordHash,
                new UserContactDto() { Email = user.UserName },
                new UserDataDto(user.FirstName, user.LastName, null, user.UserName),
                new UserPaymentDto(),
                new UserPermissionDto(),
                new EntityCreatedDto(),
                baseLink);

            return Task.FromResult(GetSecurityUserFromDomainUser(user1));
        }

        public Task DeleteAsync(SecurityUser user) {
            UserService.Delete(UserService.GetByBusinessId(Guid.Parse(user.Id)));
            return Task.FromResult<object>(null);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            //TODO
        }

        public Task<SecurityUser> FindAsync(UserLoginInfo login) {
            throw new NotImplementedException();
        }

        public Task<SecurityUser> FindByEmailAsync(string email) {
            User user = UserService.FindByEmail(email);

            return Task.FromResult(GetSecurityUserFromDomainUser(user));
        }

        public Task<SecurityUser> FindByIdAsync(string userId) {
            User domainUser = null;

            Guid userBid;
            if (Guid.TryParse(userId, out userBid)) {
                domainUser = UserService.GetByBusinessId(userBid);
            } else {
                _logger.WarnFormat("FindByIdAsync: {0} ist keine valide BID.", userId);
            }

            return Task.FromResult(GetSecurityUserFromDomainUser(domainUser));
        }

        public Task<SecurityUser> FindByNameAsync(string userName) {
            User domainUser = UserService.FindByUserName(userName);
            return Task.FromResult(GetSecurityUserFromDomainUser(domainUser));
        }

        public Task<int> GetAccessFailedCountAsync(SecurityUser user) {
            int accessFailedCount = user.AccessFailedCount;
            return Task.FromResult(accessFailedCount);
        }

        public Task<string> GetEmailAsync(SecurityUser user) {
            string email = user.Email;
            return Task.FromResult(email);
        }

        public Task<bool> GetEmailConfirmedAsync(SecurityUser user) {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(SecurityUser user) {
            bool lockOutEnabled = user.LockOutEnabled;
            return Task.FromResult(lockOutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(SecurityUser user) {
            DateTimeOffset lockOutEndDate = user.LockOutEndDate;
            return Task.FromResult(lockOutEndDate);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(SecurityUser user) {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(SecurityUser user) {
            string passwordHash = user.PasswordHash;
            return Task.FromResult(passwordHash);
        }

        public Task<bool> GetTwoFactorEnabledAsync(SecurityUser user) {
            bool twoFactorEnabled = user.TwoFactorEnabled;
            return Task.FromResult(twoFactorEnabled);
        }

        public Task<bool> HasPasswordAsync(SecurityUser user) {
            bool validPasswordExist = UserService.HasPassword(user.Id);
            return Task.FromResult(validPasswordExist);
        }

        public Task<int> IncrementAccessFailedCountAsync(SecurityUser user) {
            int accessFailedCount = user.AccessFailedCount++;
            return Task.FromResult(accessFailedCount);
        }

        public Task RemoveLoginAsync(SecurityUser user, UserLoginInfo login) {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(SecurityUser user) {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task SetEmailAsync(SecurityUser user, string email) {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(SecurityUser user, bool confirmed) {
            if (confirmed) {
                User userToUpdate = UserService.GetByBusinessId(Guid.Parse(user.Id));
                string baseLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                UserService.SetEmailConfirmed(userToUpdate, $"{baseLink}/Admin/User/{userToUpdate.BusinessId}");
            }
            return Task.FromResult(user);
        }

        public Task SetLockoutEnabledAsync(SecurityUser user, bool enabled) {
            user.LockOutEnabled = enabled;
            return Task.FromResult(enabled);
        }

        public Task SetLockoutEndDateAsync(SecurityUser user, DateTimeOffset lockoutEnd) {
            user.LockOutEndDate = lockoutEnd;
            return Task.FromResult<object>(null);
        }

        public Task SetPasswordHashAsync(SecurityUser user, string passwordHash) {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(user);
        }

        public Task SetTwoFactorEnabledAsync(SecurityUser user, bool enabled) {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(enabled);
        }

        public Task UpdateAsync(SecurityUser user) {
            User userToUpdate = UserService.GetByBusinessId(Guid.Parse(user.Id));
            UserService.Update(userToUpdate, user.UserName, user.PasswordHash);
            return Task.FromResult<object>(null);
        }

        private SecurityUser GetSecurityUserFromDomainUser(User user) {
            if (user == null) {
                /*Es wurde kein Nutzer gefunden*/
                return null;
            }
            
            if (user.IsDeleted) {
                /*Es existiert zwar ein Nutzer, dieser wurde aber gelöscht/archiviert.*/
                return null;
            }

            return new SecurityUser(user.BusinessId.ToString(),
                user.UserName,
                user.Email,
                user.PasswordHash,
                user.IsEnabled,
                user.Roles,
                user.FirstName,
                user.LastName);
        }
    }
}