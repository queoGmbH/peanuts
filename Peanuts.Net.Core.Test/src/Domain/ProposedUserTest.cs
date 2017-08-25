using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain
{
    [TestFixture]
    public class ProposedUserTest
    {
        [Test]
        public void TestProposedUserDataDto()
        {
            ProposedUserDataDto userDataDto = new ProposedUserDataDto(null, null, null, Salutation.Mister, null);
            ProposedUserDataDto otherUserDataDto = new ProposedUserDataDto("Vorname","Nachname","Titel",Salutation.Miss, new DateTime(1970,07,07));
            DtoAssert.TestEqualsAndGetHashCode(userDataDto,otherUserDataDto);
        }

        [Test]
        public void TestProposedUserContactDto()
        {
            ProposedUserContactDto userContactDto = new ProposedUserContactDto("info@queo.com", "Straße-des-1.", "1",
                "01111", "Ort 1", Country.DE, "Unternehmen 1", "http://www.1.de", "0123/456789", "0123/4567890",
                "0151/123456");
            ProposedUserContactDto userContactDto2 = new ProposedUserContactDto("info@csharp.com", "Straße-des-2.", "2", "02222", "Ort 2", Country.AT, "Unternehmen 2", "http://www.2.de", "0223/456789", "0223/4567890", "0251/123456");

            DtoAssert.TestEqualsAndGetHashCode(userContactDto,userContactDto2);
        }
        
    }
}