using System.Security.Claims;
using System.Threading.Tasks;

using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Com.QueoFlow.Peanuts.Net.Web {
    public class ApplicationSignInManager : SignInManager<SecurityUser, string> {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context) {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SecurityUser user) {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout) {

            SecurityUser user = this.UserManager.FindByNameAsync(userName).Result;

            if (user != null && user.IsEnabled == false) {
                return Task.FromResult(SignInStatus.Failure);
            }

            return base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
        }
    }
}