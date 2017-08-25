using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Infrastructure;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Controllers {
    /// <summary>
    ///     Controller für das User-Management
    /// </summary>
    [RouteArea("Admin")]
    [RoutePrefix("User")]
    [Authorization(new[] { Roles.Administrator })]
    public class UserAdministrationController : Controller {
        private ApplicationUserManager _userManager;

        public IProposedUserService ProposedUserService { get; set; }

        public IUserGroupService UserGroupService { get; set; }

        public ApplicationUserManager UserManager {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        /// <summary>
        ///     Gibt oder setzt den UserService
        /// </summary>
        public IUserService UserService { get; set; }

        /// <summary>
        ///     Controller-Methode für das Erstellen eines Users
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateCommand userCreateCommand, User currentUser, ProposedUser proposedUser) {
            Require.NotNull(userCreateCommand, "userCreateCommand");

            if (ModelState.IsValid) {
                IdentityResult identityResult = UserManager.PasswordValidator.ValidateAsync(userCreateCommand.Password).Result;

                if (identityResult.Succeeded) {
                    string passwordHash = UserManager.PasswordHasher.HashPassword(userCreateCommand.Password);

                    UserService.Create(passwordHash,
                        userCreateCommand.UserContactDto,
                        userCreateCommand.UserDataDto,
                        userCreateCommand.UserPaymentDto,
                        userCreateCommand.UserPermissionDto,
                        new EntityCreatedDto(currentUser, DateTime.Now));
                    if (proposedUser != null) {
                        ProposedUserService.Delete(proposedUser);
                    }

                    return RedirectToAction("Index", "UserAdministration");
                }
                AddErrors(identityResult);
            }
            UserCreateViewModel userCreateViewModel = new UserCreateViewModel(Roles.AllRoles,
                userCreateCommand,
                UserGroupService.GetAll(),
                proposedUser);
            return View(userCreateViewModel);
        }

        /// <summary>
        ///     Controller-Methode für das Anzeigen der View zum Erstellen eines Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Create")]
        public ActionResult CreateForm() {
            IList<string> availableRoles = Roles.AllRoles;
            IList<UserGroup> financialBrokerPools = UserGroupService.GetAll();
            UserCreateViewModel userCreateViewModel = new UserCreateViewModel(availableRoles, new UserCreateCommand(), financialBrokerPools);
            return View("Create", userCreateViewModel);
        }

        /// <summary>
        ///     Controller-Methode für das Löschen eines Users
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{user:guid}")]
        public ActionResult Delete(User user) {
            Require.NotNull(user, "user");

            UserService.Delete(user);
            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Controller-Methode für das Löschen eines Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{user:guid}/Delete")]
        public ActionResult DeleteForm(User user) {
            Require.NotNull(user, "user");

            return View("Delete", new UserDeleteViewModel(user));
        }

        [Authorization(Roles.Administrator)]
        [Route("{user:guid}/Impersonate")]
        public ActionResult Impersonate(User user) {
            Require.NotNull(user, "user");

            /*Impersonifizierung aktivieren*/
            //SecurityContext.Current.EnableImpersonation(user);

            /*Erstmal auf Startseite gehen*/
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        /// <summary>
        ///     Controller-Methode für das Anzeigen Aller UserShow
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public ActionResult Index(string search, PaginationCommand users) {
            IPage<User> resultPageUsers = UserService.FindUser(users, search);
            IList<ProposedUser> proposedUsers = ProposedUserService.GetAll();
            UserListViewModel userListModel = new UserListViewModel(resultPageUsers, search, proposedUsers);
            return View(userListModel);
        }

        [AllowAnonymous]
        [Route("Personate")]
        public ActionResult Personate() {
            /*Impersonifizierung aktivieren*/
            //SecurityContext.Current.DisableImpersonation();

            /*Erstmal auf Startseite gehen*/
            return RedirectToAction("Index", "UserAdministration", new { area = "Admin" });
        }

        /// <summary>
        ///     Controller-Methode für das Anzeigen eines Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{user:guid}")]
        public ActionResult Show(User user) {
            Require.NotNull(user, "user");

            UserShowViewModel userShowViewModel = new UserShowViewModel(user);
            return View(userShowViewModel);
        }

        /// <summary>
        ///     Controller-Methode für das Bearbeiten eines Users
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{user:guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Update(User user, UserUpdateCommand userUpdateCommand, User currentUser) {
            Require.NotNull(user, "user");
            Require.NotNull(userUpdateCommand, "userUpdateCommand");

            /*Wenn ein neues Passwort eingetragen wurde und valide ist, dann wird es übernommen.*/
            string passwordHash = user.PasswordHash;
            if (!string.IsNullOrWhiteSpace(userUpdateCommand.NewPassword)) {
                /*neues Passwort validieren*/
                IdentityResult identityResult = UserManager.PasswordValidator.ValidateAsync(userUpdateCommand.NewPassword).Result;
                if (identityResult.Succeeded) {
                    /*Passwort-Hash erstellen*/
                    passwordHash = UserManager.PasswordHasher.HashPassword(userUpdateCommand.NewPassword);
                } else {
                    /*Fehler am Model-State hinterlegen, damit die Update-Routine abgebrochen wird und eine Fehlermeldung auf dem Formular angezeigt wird*/
                    AddErrors(identityResult, "Password");
                }
            }

            if (ModelState.IsValid) {
                /*Wenn alle Eingaben valide sind, dann Nutzer aktualisieren*/
                UserService.Update(user,
                    passwordHash,
                    userUpdateCommand.UserContactDto,
                    userUpdateCommand.UserDataDto,
                    userUpdateCommand.UserPaymentDto,
                    userUpdateCommand.UserNotificationOptionsDto,
                    userUpdateCommand.UserPermissionDto,
                    userUpdateCommand.NewDocuments,
                    userUpdateCommand.DeleteDocuments,
                    new EntityChangedDto(currentUser, DateTime.Now));
                return RedirectToAction("Index", "UserAdministration");
            }
            /*andernfalls Formular wieder anzeigen*/
            IList<string> roles = Roles.AllRoles;
            IList<UserGroup> financialBrokerPools = UserGroupService.GetAll();
            UserUpdateViewModel userUpdateViewModel = new UserUpdateViewModel(user, roles, financialBrokerPools, userUpdateCommand);
            return View(userUpdateViewModel);
        }

        /// <summary>
        ///     Controller-Methode für das Anzeigen der Edit-View eines Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{user:guid}/Update")]
        public ActionResult UpdateForm(User user) {
            Require.NotNull(user, "user");

            UserUpdateCommand userUpdateCommand = new UserUpdateCommand(user);
            IList<string> roles = Roles.AllRoles;
            IList<UserGroup> financialBrokerPools = UserGroupService.GetAll();
            UserUpdateViewModel userUpdateViewModel = new UserUpdateViewModel(user, roles, financialBrokerPools, userUpdateCommand);
            return View("Update", userUpdateViewModel);
        }

        private void AddErrors(IdentityResult result, string modelKey = "") {
            foreach (string error in result.Errors) {
                ModelState.AddModelError(modelKey, error);
            }
        }
    }
}