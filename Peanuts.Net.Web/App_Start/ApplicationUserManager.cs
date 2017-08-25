using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

using Spring.Context.Support;

namespace Com.QueoFlow.Peanuts.Net.Web {
    public class ApplicationUserManager : UserManager<SecurityUser> {
        public ApplicationUserManager(UserStoreAdapter store, IUserService userService)
            : base(store) {
            UserService = userService;
        }

        private IUserService UserService { get; set; }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) {
            //TODO: Schöner wäre: Über Owin auf Spring-Context zugreifen
            UserStoreAdapter userStoreAdapter = ContextRegistry.GetContext().GetObject<UserStoreAdapter>();
            IUserService userService = ContextRegistry.GetContext().GetObject<IUserService>();
            var manager = new ApplicationUserManager(userStoreAdapter, userService);
            // Konfigurieren der Überprüfungslogik für Benutzernamen.
            manager.UserValidator = new UserValidator<SecurityUser>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Konfigurieren der Überprüfungslogik für Kennwörter.
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            // Standardeinstellungen für Benutzersperren konfigurieren
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 10;

            // Registrieren von Anbietern für zweistufige Authentifizierung. Diese Anwendung verwendet telefonische und E-Mail-Nachrichten zum Empfangen eines Codes zum Überprüfen des Benutzers.
            // Sie können Ihren eigenen Anbieter erstellen und hier einfügen.
            //manager.RegisterTwoFactorProvider("Telefoncode", new PhoneNumberTokenProvider<SecurityUser>
            //{
            //    MessageFormat = "Ihr Sicherheitscode lautet {0}"
            //});
            //manager.RegisterTwoFactorProvider("E-Mail-Code", new EmailTokenProvider<SecurityUser>
            //{
            //    Subject = "Sicherheitscode",
            //    BodyFormat = "Ihr Sicherheitscode lautet {0}"
            //});
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null) {
                manager.UserTokenProvider =
                        new DataProtectorTokenProvider<SecurityUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(SecurityUser user, string authenticationType) {
            ClaimsIdentity claimsIdentity = await base.CreateIdentityAsync(user, authenticationType);
            User fullUserObject = UserService.GetByBusinessId(Guid.Parse(user.Id));
            // Die restlichen Daten hier befüllen.
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, fullUserObject.FirstName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, fullUserObject.LastName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, fullUserObject.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, fullUserObject.UserName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.PrimarySid, fullUserObject.BusinessId.ToString()));
            // Berechtigungen bestimmen
            AuthorizationManager authenticationManager = new AuthorizationManager(fullUserObject);
            IList<IGrantedAuthority> grantedAuthorities = authenticationManager.GetAuthorities();
            foreach (IGrantedAuthority grantedAuthority in grantedAuthorities) {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, grantedAuthority.Authority));
            }
            return claimsIdentity;
        }
    }
}