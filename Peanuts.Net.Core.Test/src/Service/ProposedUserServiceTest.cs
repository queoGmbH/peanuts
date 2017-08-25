using System;

using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Service
{
    [TestFixture]
    public class ProposedUserServiceTest : ServiceBaseTest
    {
        public IProposedUserService ProposedUserService { get; set; }

        public ProposedUserCreator ProposedUserCreator { get; set; }

        public UserCreator UserCreator { get; set; }

        public UserGroupCreator UserGroupCreator { get; set; }

        [Test]
        public void TestCreateAndLoadProposedUser()
        {
            // when:
            ProposedUserContactDto updatedProposedUserContactDto = ProposedUserCreator.CreateProposedUserContactDto("testi@test.com", "Testunternehmen", "Teststraße", "?", "01234", "Teststadt",
                    Country.DE, "http://www.url.test", "phone", "privat", "mobile", "fax");
            ProposedUserDataDto updatedProposedUserDataDto = ProposedUserCreator.CreateProposedUserDataDto("Vorname", "Nachname",  Salutation.Mister, "Titel", new DateTime(1980,01,02));

            EntityCreatedDto creationDto = new EntityCreatedDto(UserCreator.Create(), new DateTime(2016, 12, 12, 15, 30, 0));
            ProposedUser user = ProposedUserService.Create("testNutzer", updatedProposedUserDataDto, updatedProposedUserContactDto, creationDto);

            ProposedUser actualUser = ProposedUserService.GetByBusinessId(user.BusinessId);

            // then:
            Assert.AreEqual(user, actualUser);
            DtoAssert.AreEqual(updatedProposedUserDataDto,actualUser.GetUserDataDto());
            DtoAssert.AreEqual(updatedProposedUserContactDto,actualUser.GetUserContactDto());
        }
    }
}