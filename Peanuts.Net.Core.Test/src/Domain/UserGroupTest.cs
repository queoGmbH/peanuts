using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain {
    [TestFixture]
    public class UserGroupTest {
        [Test]
        public void TestConstructor() {
            // given:
            string expectedAdditionalInformations = "Keine";
            string expectedName = "die firma";
            int expectedAcceptedBalance = -10;
            
            UserGroupDto userGroupDto = new UserGroupDto(expectedAdditionalInformations, expectedName, expectedAcceptedBalance);

            User createdBy = new User();
            DateTime createdAt = new DateTime(2017, 1, 10);
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(createdBy, createdAt);
            // when:
            UserGroup actualBrokerPool = new UserGroup(userGroupDto, entityCreatedDto);
            // then:
            actualBrokerPool.GetDto().ShouldBeEquivalentTo(userGroupDto);
            actualBrokerPool.AdditionalInformations.ShouldBeEquivalentTo(expectedAdditionalInformations);
            actualBrokerPool.ChangedAt.Should().Be(null);
            actualBrokerPool.ChangedBy.Should().BeNull();
            actualBrokerPool.CreatedAt.ShouldBeEquivalentTo(createdAt);
            actualBrokerPool.CreatedBy.ShouldBeEquivalentTo(createdBy);
            actualBrokerPool.Name.ShouldBeEquivalentTo(expectedName);
        }

        [Test]
        public void TestEqualsAndGetHashCodeFromDto() {
            // given:
            UserGroupDto firstDto = new UserGroupDto("Keine", "der Name 1",-10);
            UserGroupDto secondDto = new UserGroupDto(null, "der name 2", -20);
            // when, then:
            DtoAssert.TestEqualsAndGetHashCode(firstDto, secondDto);
        }
    }
}