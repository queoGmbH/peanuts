using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    [TestFixture]
    public class UserGroupDaoTest : PersistenceBaseTest {
        public UserGroupCreator UserGroupCreator { get; set; }

        public IUserGroupDao UserGroupDao { get; set; }

        public UserCreator UserCreator { get; set; }
        
        [Test]
        public void TestSaveAndLoad() {
            // given:
            string expectedAdditionalInformations = "Keine";
            string expectedName = "die firma";
            int acceptedBalance = -10;
            
            UserGroupDto userGroupDto = new UserGroupDto(expectedAdditionalInformations, expectedName, acceptedBalance);

            DateTime createdAt = new DateTime(2016, 12, 12);
            User createdBy = UserCreator.Create();
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(createdBy, createdAt);
            UserGroup userGroup = new UserGroup(userGroupDto, entityCreatedDto);
            // when:
            userGroup = UserGroupDao.Save(userGroup);
            UserGroupDao.FlushAndClear();
            UserGroup actualBrokerPool = UserGroupDao.Get(userGroup.Id);
            // then:
            actualBrokerPool.GetDto().ShouldBeEquivalentTo(userGroupDto);
            actualBrokerPool.AdditionalInformations.ShouldBeEquivalentTo(expectedAdditionalInformations);
            actualBrokerPool.ChangedAt.Should().Be(null);
            actualBrokerPool.ChangedBy.Should().BeNull();
            actualBrokerPool.CreatedAt.ShouldBeEquivalentTo(createdAt);
            actualBrokerPool.CreatedBy.ShouldBeEquivalentTo(createdBy);
            actualBrokerPool.Name.ShouldBeEquivalentTo(expectedName);
            actualBrokerPool.BalanceOverdraftLimit.ShouldBeEquivalentTo(acceptedBalance);
        }

        [Test]
        public void TestUpdate() {
            // given:
            UserGroup userGroupToUpdate = UserGroupCreator.Create();

            string expectedAdditionalInformations = "Keine";
            string expectedName = "die firma";
            int expectedAcceptedBalance = -20;
            UserGroupDto userGroupDtoForUpdate = new UserGroupDto(expectedAdditionalInformations, expectedName, expectedAcceptedBalance);
            User changedBy = UserCreator.Create("changed by");
            DateTime changedAt = new DateTime(2016, 12, 22);
            EntityChangedDto entityChangedDto = new EntityChangedDto(changedBy, changedAt);
            // when:
            userGroupToUpdate.Update(userGroupDtoForUpdate, entityChangedDto);
            UserGroupDao.FlushAndClear();
            UserGroup actualBrokerPool = UserGroupDao.Get(userGroupToUpdate.Id);
            // then: 
            actualBrokerPool.GetDto().ShouldBeEquivalentTo(userGroupDtoForUpdate);
            actualBrokerPool.AdditionalInformations.ShouldBeEquivalentTo(expectedAdditionalInformations);
            actualBrokerPool.BalanceOverdraftLimit.ShouldBeEquivalentTo(expectedAcceptedBalance);
            actualBrokerPool.ChangedAt.Should().Be(changedAt);
            actualBrokerPool.ChangedBy.Should().Be(changedBy);
            actualBrokerPool.Name.ShouldBeEquivalentTo(expectedName);            
        }

        [Test]
        public void TestAreUsersAssigned() {
            // given:
            UserGroup userGroup = UserGroupCreator.Create();
            User user= UserCreator.Create("ein nutzer");
            UserGroupDao.Save(new UserGroupMembership(userGroup, user, UserGroupMembershipType.Member, new EntityCreatedDto(user, DateTime.Now)));

            // when:
            bool actualAreUsersAssigned = UserGroupDao.AreUsersAssigned(userGroup);

            // then:
            actualAreUsersAssigned.Should().BeTrue();
        }
    }
}