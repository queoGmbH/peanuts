using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.UserGroup;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Controllers {
    [RouteArea("Admin")]
    [RoutePrefix("UserGroup")]
    [Authorization(new[] { Roles.Administrator })]
    public class UserGroupAdministrationController : Controller {
        
        /// <summary>
        ///     Liefert oder setzt die Implementierung für den <see cref="IUserGroupService" />.
        /// </summary>
        public IUserGroupService UserGroupService { get; set; }

        public IUserService UserService { get; set; }

        [Route("")]
        [HttpPost]
        public ActionResult Create(UserGroupCreateCommand userGroupCreateCommand, User currentUser) {
            Require.NotNull(currentUser, nameof(currentUser));
            if (!ModelState.IsValid) {
                UserGroupCreateViewModel userGroupCreateViewModel = GetFinancialBrokerPoolCreateViewModel(userGroupCreateCommand);
                return View(userGroupCreateViewModel);
            }

            // TODO: Initiale Nutzer!
            UserGroupService.Create(userGroupCreateCommand.UserGroupDto, new Dictionary<User, UserGroupMembershipType>(), currentUser);

            return RedirectToAction("Index");
        }

        [Route("Create")]
        [HttpGet]
        public ActionResult CreateForm() {
            UserGroupCreateCommand userGroupCreateCommand = new UserGroupCreateCommand();
            UserGroupCreateViewModel userGroupCreateViewModel = GetFinancialBrokerPoolCreateViewModel(userGroupCreateCommand);
            return View("Create", userGroupCreateViewModel);
        }

        [Route("{userGroup}")]
        [HttpDelete]
        public ActionResult Delete(UserGroup userGroup) {
            Require.NotNull(userGroup, nameof(userGroup));
            UserGroupService.Delete(userGroup);

            return RedirectToAction("Index");
        }

        [Route("{userGroup}/Delete")]
        [HttpGet]
        public ActionResult DeleteForm(UserGroup userGroup) {
            Require.NotNull(userGroup, nameof(userGroup));
            UserGroupDeleteViewModel userGroupDeleteViewModel = new UserGroupDeleteViewModel(userGroup);
            if (UserGroupService.AreUsersAssigned(userGroup)) {
                return View("CanNotDelete", userGroupDeleteViewModel);
            }
            return View("Delete", userGroupDeleteViewModel);
        }

        [Route("")]
        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 25) {
            PageRequest pageRequest = new PageRequest(page, pageSize);
            IPage<UserGroup> financialBrokerPools = UserGroupService.GetAll(pageRequest);
            UserGroupListViewModel userGroupListViewModel = new UserGroupListViewModel(financialBrokerPools);
            return View("Index", userGroupListViewModel);
        }

        [Route("{userGroup}")]
        [HttpGet]
        public ActionResult Show(UserGroup userGroup) {
            Require.NotNull(userGroup, nameof(userGroup));

    
            IList<UserGroupMembership> members =
                    UserGroupService.FindMembershipsByGroups(PageRequest.All,
                        new List<UserGroup>() { userGroup },
                        new List<UserGroupMembershipType>() {
                            UserGroupMembershipType.Administrator, UserGroupMembershipType.Member, UserGroupMembershipType.Invited,
                            UserGroupMembershipType.Quit, UserGroupMembershipType.Request
                        }).ToList();
            UserGroupShowViewModel userGroupShowViewModel = new UserGroupShowViewModel(userGroup, members);
            return View(userGroupShowViewModel);
        }

        [Route("{userGroup}")]
        [HttpPut]
        public ActionResult Update(UserGroup userGroup, UserGroupUpdateCommand userGroupUpdateCommand, User currentUser) {
            Require.NotNull(userGroup, nameof(userGroup));
            Require.NotNull(currentUser, nameof(currentUser));
            if (!ModelState.IsValid) {
                UserGroupUpdateViewModel brokerPoolUpdateViewModel = GetFinancialBrokerPoolUpdateViewModel(userGroupUpdateCommand);
                return View(brokerPoolUpdateViewModel);
            }
            UserGroupService.Update(userGroup, userGroupUpdateCommand.UserGroupDto, currentUser);

            return RedirectToAction("Index");
        }

        [Route("{userGroup}/Update")]
        [HttpGet]
        public ActionResult UpdateForm(UserGroup userGroup) {
            Require.NotNull(userGroup, nameof(userGroup));
            UserGroupDto userGroupDto = userGroup.GetDto();
            UserGroupUpdateCommand userGroupUpdateCommand = new UserGroupUpdateCommand(userGroupDto);
            UserGroupUpdateViewModel userGroupUpdateViewModel =
                    GetFinancialBrokerPoolUpdateViewModel(userGroupUpdateCommand);
            return View("Update", userGroupUpdateViewModel);
        }

        private UserGroupCreateViewModel GetFinancialBrokerPoolCreateViewModel(
            UserGroupCreateCommand userGroupCreateCommand) {
            // TODO: Prüfen welche Nutzer in der Liste zur Verfügung stehen sollen!
            IList<User> users = UserService.GetAll();
            UserGroupCreateViewModel userGroupCreateViewModel =
                    new UserGroupCreateViewModel(userGroupCreateCommand, users);
            return userGroupCreateViewModel;
        }

        private UserGroupUpdateViewModel GetFinancialBrokerPoolUpdateViewModel(UserGroupUpdateCommand userGroupUpdateCommand) {
            IList<User> users = UserService.GetAll();
            UserGroupUpdateViewModel userGroupUpdateViewModel = new UserGroupUpdateViewModel(userGroupUpdateCommand, users);
            return userGroupUpdateViewModel;
        }
    }
}