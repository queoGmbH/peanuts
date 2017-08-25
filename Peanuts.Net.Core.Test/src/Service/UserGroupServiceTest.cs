using System.Collections.Generic;
using System.Linq;
using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    [TestFixture]
    public class UserGroupServiceTest : ServiceBaseTest {
        public UserGroupCreator UserGroupCreator { get; set; }

        public UserCreator UserCreator { get; set; }

        public IUserGroupService UserGroupService { get; set; }

        [Test]
        public void TestAddMembers() {
            //given:
            User user1 = UserCreator.Create();
            User user2 = UserCreator.Create();

            UserGroup userGroup = UserGroupCreator.Create("", "Gruppe 1");
            Dictionary<User, UserGroupMembershipType> users = new Dictionary<User, UserGroupMembershipType>();
            users.Add(user1, UserGroupMembershipType.Administrator);
            users.Add(user2, UserGroupMembershipType.Member);

            //when:
            IList<UserGroupMembership> userGroupMemberships = UserGroupService.AddMembers(userGroup, users, user1);

            //Then:
            Assert.AreEqual(2, userGroupMemberships.Count);
        }

        [Test]
        public void TestAreUsersAssigned() {
            //given
            User user1 = UserCreator.Create();

            UserGroup userGroup = UserGroupCreator.Create("", "Gruppe1");
            UserGroup userGroup2 = UserGroupCreator.Create();

            Dictionary<User, UserGroupMembershipType> users = new Dictionary<User, UserGroupMembershipType>();
            users.Add(user1, UserGroupMembershipType.Administrator);

            //when:
            UserGroupService.AddMembers(userGroup, users, user1);

            //then:
            Assert.IsTrue(UserGroupService.AreUsersAssigned(userGroup));
            Assert.IsFalse(UserGroupService.AreUsersAssigned(userGroup2));
        }

        [Test]
        public void TestCreateAndLoadUserGroup() {
            //given: 
            User user = UserCreator.Create();
            UserGroupDto userGroupDto = new UserGroupDto("keine", "Gruppe 1", -10);
            Dictionary<User, UserGroupMembershipType> intitalUsers = new Dictionary<User, UserGroupMembershipType>();
            intitalUsers.Add(user, UserGroupMembershipType.Administrator);

            //when:
            UserGroup userGroup = UserGroupService.Create(userGroupDto, intitalUsers, user);
            UserGroup actualUserGroup = UserGroupService.GetAll().First(x => x.BusinessId == userGroup.BusinessId);

            //then:
            Assert.AreEqual(userGroup, actualUserGroup);
            DtoAssert.AreEqual(userGroupDto, actualUserGroup.GetDto());
        }

        [Test]
        public void TestFindMembershipByUser() {
            //given:
            User user1 = UserCreator.Create();
            User user2 = UserCreator.Create();
            User user3 = UserCreator.Create();

            UserGroup userGroup1 = UserGroupCreator.Create();
            UserGroup userGroup2 = UserGroupCreator.Create();

            Dictionary<User, UserGroupMembershipType> users1 = new Dictionary<User, UserGroupMembershipType>();
            users1.Add(user1, UserGroupMembershipType.Administrator);
            users1.Add(user2, UserGroupMembershipType.Member);
            users1.Add(user3, UserGroupMembershipType.Invited);

            Dictionary<User, UserGroupMembershipType> users2 = new Dictionary<User, UserGroupMembershipType>();
            users2.Add(user2, UserGroupMembershipType.Administrator);
            users2.Add(user3, UserGroupMembershipType.Request);

            UserGroupService.AddMembers(userGroup1, users1, user1);
            UserGroupService.AddMembers(userGroup2, users2, user2);

            //when:
            List<UserGroupMembership> user1Memberships = UserGroupService.FindMembershipsByUser(PageRequest.All, user1).ToList();
            List<UserGroupMembership> user2Memberships = UserGroupService.FindMembershipsByUser(PageRequest.All, user2,
                new List<UserGroupMembershipType> {UserGroupMembershipType.Member}).ToList();
            List<UserGroupMembership> user3Memberships =
                UserGroupService.FindMembershipsByUser(PageRequest.All, user3,
                    new List<UserGroupMembershipType> {UserGroupMembershipType.Request, UserGroupMembershipType.Invited}).ToList();

            //then:
            Assert.AreEqual(1, user1Memberships.Count);
            Assert.AreEqual(1, user2Memberships.Count);
            Assert.AreEqual(2, user3Memberships.Count);
        }

        [Test]
        public void TestInvite() {
            //given
            User user = UserCreator.Create();
            User user2 = UserCreator.Create();
            UserGroup userGroup = UserGroupCreator.Create();

            UserGroupService.Invite(userGroup, user2, user, "");

            UserGroupMembership invitations =
                UserGroupService.FindMembershipsByUser(PageRequest.All, user2, new List<UserGroupMembershipType> {UserGroupMembershipType.Invited})
                    .FirstOrDefault();

            Assert.AreEqual(UserGroupMembershipType.Invited, invitations.MembershipType);
        }

        [Test]
        public void TestIsUserSolvent() {
            //given:
            User user = UserCreator.Create();
            User user2 = UserCreator.Create();
            UserGroup userGroup = UserGroupCreator.Create("", "Gruppe 1", -20);
            Dictionary<User, UserGroupMembershipType> inititalUsers = new Dictionary<User, UserGroupMembershipType>();
            inititalUsers.Add(user, UserGroupMembershipType.Administrator);
            inititalUsers.Add(user2, UserGroupMembershipType.Member);

            IList<UserGroupMembership> userGroupMemberships = UserGroupService.AddMembers(userGroup, inititalUsers, user);
            UserGroupMembership userGroupMembership = UserGroupService.FindMembershipsByUser(PageRequest.All, user).FirstOrDefault();
            UserGroupMembership user2GroupMembership = UserGroupService.FindMembershipsByUser(PageRequest.All, user2).FirstOrDefault();

            //when:
            userGroupMembership.Account.Book(-30);
            user2GroupMembership.Account.Book(-10);

            //then:
            Assert.IsFalse(UserGroupService.IsUserSolvent(userGroupMembership));
            Assert.IsTrue(UserGroupService.IsUserSolvent(user2GroupMembership));
        }

        [Test]
        public void TestIsUserSolvent2() {
            //given:
            User user = UserCreator.Create();
            User user2 = UserCreator.Create();
            UserGroup userGroup = UserGroupCreator.Create("", "Gruppe 1", null);
            Dictionary<User, UserGroupMembershipType> inititalUsers = new Dictionary<User, UserGroupMembershipType>();
            inititalUsers.Add(user, UserGroupMembershipType.Administrator);
            inititalUsers.Add(user2, UserGroupMembershipType.Member);

            IList<UserGroupMembership> userGroupMemberships = UserGroupService.AddMembers(userGroup, inititalUsers, user);
            UserGroupMembership userGroupMembership = UserGroupService.FindMembershipsByUser(PageRequest.All, user).FirstOrDefault();
            UserGroupMembership user2GroupMembership = UserGroupService.FindMembershipsByUser(PageRequest.All, user2).FirstOrDefault();

            //when:
            userGroupMembership.Account.Book(-30);
            user2GroupMembership.Account.Book(-10);

            //then:
            Assert.IsTrue(UserGroupService.IsUserSolvent(userGroupMembership));
            Assert.IsTrue(UserGroupService.IsUserSolvent(user2GroupMembership));
        }

        [Test]
        public void TestRequestMembership() {
            User user1 = UserCreator.Create();
            UserGroup userGroup = UserGroupCreator.Create();

            UserGroupService.RequestMembership(userGroup, user1, "");

            List<UserGroupMembership> users = UserGroupService.FindMembershipsByUser(PageRequest.All, user1,
                new List<UserGroupMembershipType> {UserGroupMembershipType.Request}).ToList();
            UserGroupMembership membership = users.FirstOrDefault();

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(UserGroupMembershipType.Request, membership.MembershipType);
        }
    }
}