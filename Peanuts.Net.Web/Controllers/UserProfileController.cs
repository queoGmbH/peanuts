using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Web.Models.Manage;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [Authorize]
    [RoutePrefix("UserProfile")]
    public class UserProfileController : Controller {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserProfileController() {
        }

        public UserProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public IUserService UserService { get; set; }

        //
        // GET: /Manage/AddPhoneNumber
        [HttpGet]
        [Route("AddPhoneNumber")]
        public ActionResult AddPhoneNumber() {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [Route("AddPhoneNumber")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            // Token generieren und senden
            string code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null) {
                IdentityMessage message = new IdentityMessage {
                    Destination = model.Number,
                    Body = "Ihr Sicherheitscode lautet " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        [Route("ChangePassword")]
        public ActionResult ChangePassword() {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded) {
                SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null) {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [Route("DisableTwoFactorAuthentication")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication() {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null) {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "UserProfile");
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [Route("EnableTwoFactorAuthentication")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication() {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null) {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "UserProfile");
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Index(ManageMessageId? message, User currentUser) {
            //ViewBag.StatusMessage =
            //    message == ManageMessageId.ChangePasswordSuccess ? "Ihr Kennwort wurde geändert."
            //    : message == ManageMessageId.SetPasswordSuccess ? "Ihr Kennwort wurde festgelegt."
            //    : message == ManageMessageId.SetTwoFactorSuccess ? "Ihr Anbieter für zweistufige Authentifizierung wurde festgelegt."
            //    : message == ManageMessageId.Error ? "Fehler"
            //    : message == ManageMessageId.AddPhoneSuccess ? "Ihre Telefonnummer wurde hinzugefügt."
            //    : message == ManageMessageId.RemovePhoneSuccess ? "Ihre Telefonnummer wurde entfernt."
            //    : "";

            //var userId = User.Identity.GetUserId();
            //var model = new IndexViewModel
            //{
            //    HasPassword = HasPassword(),
            //    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
            //    User = new UserShowViewModel(currentUser),
            //    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            //};

            ShowMeViewModel showMeViewModel = new ShowMeViewModel(currentUser);

            return View(showMeViewModel);
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [Route("LinkLogin")]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider) {
            // Umleitung an den externen Anmeldeanbieter anfordern, um eine Anmeldung für den aktuellen Benutzer zu verknüpfen.
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "UserProfile"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        [Route("LinkLoginCallback")]
        public async Task<ActionResult> LinkLoginCallback() {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null) {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ManageLogins
        [HttpGet]
        [Route("ManageLogins")]
        public async Task<ActionResult> ManageLogins(ManageMessageId? message) {
            ViewBag.StatusMessage =
                    message == ManageMessageId.RemoveLoginSuccess ? "Die externe Anmeldung wurde entfernt."
                        : message == ManageMessageId.Error ? "Fehler"
                            : "";
            SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) {
                return View("Error");
            }
            IList<UserLoginInfo> userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            List<AuthenticationDescription> otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/RemoveLogin
        [Route("RemoveLogin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey) {
            ManageMessageId? message;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded) {
                SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null) {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            } else {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [Route("RemovePhoneNumber")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber() {
            IdentityResult result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded) {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null) {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        [Route("SetPassword")]
        public ActionResult SetPassword() {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [Route("SetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model) {
            if (ModelState.IsValid) {
                IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded) {
                    SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null) {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten. Formular erneut anzeigen.
            return View(model);
        }

        [HttpPatch]
        [Route]
        [ValidateAntiForgeryToken]
        public ActionResult Update(User currentUser, UpdateMeCommand updateMeCommand) {
            if (ModelState.IsValid) {
                UserService.Update(currentUser, updateMeCommand.UserContactDto, updateMeCommand.UserDataDto, updateMeCommand.UserPaymentDto, updateMeCommand.UserNotificationOptionsDto, new EntityChangedDto(currentUser, DateTime.Now));
                return RedirectToAction("Index");
            }

            UpdateMeViewModel updateMeViewModel = new UpdateMeViewModel(currentUser, updateMeCommand);
            return View(updateMeViewModel);
        }

        [HttpGet]
        [Route("Update")]
        public ActionResult UpdateForm(User currentUser) {
            UpdateMeViewModel userUpdateViewModel = new UpdateMeViewModel(currentUser);
            return View("Update", userUpdateViewModel);
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        [Route("VerifyPhoneNumber")]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber) {
            string code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Eine SMS über den SMS-Anbieter senden, um die Telefonnummer zu überprüfen.
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [Route("VerifyPhoneNumber")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            IdentityResult result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded) {
                SecurityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null) {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten. Formular erneut anzeigen.
            ModelState.AddModelError("", "Fehler beim Überprüfen des Telefons.");
            return View(model);
        }

        protected override void Dispose(bool disposing) {
            if (disposing && _userManager != null) {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Hilfsprogramme

        // Wird für XSRF-Schutz beim Hinzufügen externer Anmeldungen verwendet.
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result) {
            foreach (string error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword() {
            SecurityUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null) {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber() {
            SecurityUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null) {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}