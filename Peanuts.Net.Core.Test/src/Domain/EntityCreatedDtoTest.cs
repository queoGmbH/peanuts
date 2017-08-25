using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain {
    [TestFixture]
    public class EntityCreatedDtoTest {
        [Test]
        public void TestDto() {
            // given:
            User user = new User();
            User otherUser = new User();

            // when:
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(user, new DateTime(2017, 1, 1));
            EntityCreatedDto otherEntityCreatedDto = new EntityCreatedDto(otherUser, new DateTime(2017, 1, 11));

            // then:
            DtoAssert.TestEqualsAndGetHashCode(entityCreatedDto, otherEntityCreatedDto);
        }
    }
}