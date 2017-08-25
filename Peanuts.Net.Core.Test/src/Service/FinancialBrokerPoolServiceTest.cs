using System;

using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

using Moq;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    [TestFixture]
    public class FinancialBrokerPoolServiceTest : ServiceBaseTest {
        public UserGroupCreator UserGroupCreator { get; set; }

        [Test]
        public void TestDelete() {
            // given:
            Mock<IUserGroupDao> financialBrokerPoolDaoMock = new Mock<IUserGroupDao>();
            financialBrokerPoolDaoMock.Setup(m => m.AreUsersAssigned(It.IsAny<UserGroup>())).Returns(false);
            UserGroupService userGroupService = new UserGroupService(financialBrokerPoolDaoMock.Object);
            UserGroup userGroup = UserGroupCreator.Create();
            // when:
            userGroupService.Delete(userGroup);
            // then:
            financialBrokerPoolDaoMock.Verify(m => m.Delete(It.IsAny<UserGroup>()), Times.Once);
        }

        [Test]
        public void TestDeleteWhenAssignedUserExists() {
            // given:
            Mock<IUserGroupDao> financialBrokerPoolDaoMock = new Mock<IUserGroupDao>();
            financialBrokerPoolDaoMock.Setup(m => m.AreUsersAssigned(It.IsAny<UserGroup>())).Returns(true);
            UserGroupService userGroupService = new UserGroupService(financialBrokerPoolDaoMock.Object);
            UserGroup userGroup = UserGroupCreator.Create();
            // when, then:
            Assert.Throws<InvalidOperationException>(() => userGroupService.Delete(userGroup));
            financialBrokerPoolDaoMock.Verify(m => m.Delete(It.IsAny<UserGroup>()), Times.Never);
        }
    }
}