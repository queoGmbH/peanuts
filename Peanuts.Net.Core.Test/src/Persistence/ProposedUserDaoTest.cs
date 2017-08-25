using System;

using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

using FluentAssertions;

using NHibernate;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    [TestFixture]
    public class ProposedUserDaoTest : PersistenceBaseTest {
        public ProposedUserDao ProposedUserDao { get; set; }

        public ProposedUserCreator ProposedUserCreator { get; set; }

        public UserCreator UserCreator { get; set; }

        public UserGroupCreator UserGroupCreator { get; set; }

        [Test]
        public void TestDeleteUser() {
            //given:
            ProposedUser user = ProposedUserCreator.CreateProposedUser();
            // when:
            ProposedUser actualUser = ProposedUserDao.Get(user.Id);
            ProposedUserDao.Delete(actualUser);
            ProposedUserDao.FlushAndClear();

            // then: Exception
            Assert.Throws<ObjectNotFoundException>(() => ProposedUserDao.Get(user.Id));
        }

        [Test]
        public void TestGetUserByBusinessId() {
            // given:
            ProposedUser user = ProposedUserCreator.CreateProposedUser();
            // when:
            ProposedUser actualUser = ProposedUserDao.GetByBusinessId(user.BusinessId);
            // then:
            Assert.AreEqual(user, actualUser);
        }

        [Test]
        public void TestGetUserByEmail() {
            // given:
            ProposedUser user = ProposedUserCreator.CreateProposedUser();
            // when:
            ProposedUser actualUser = ProposedUserDao.FindByEmail(user.Email);
            // then:
            Assert.AreEqual(user, actualUser);
        }

        [Test]
        public void TestGetUserByUserName() {
            // given:
            ProposedUser user = ProposedUserCreator.CreateProposedUser();
            // when:
            ProposedUser actualUser = ProposedUserDao.FindByUserName(user.UserName);
            // then:
            Assert.AreEqual(user, actualUser);
        }

        [Test]
        public void TestSaveAndLoad() {
            // given :
            UserGroup userGroup = UserGroupCreator.Create();
            ProposedUserDataDto proposedUserDataDto = new ProposedUserDataDto("Beantragter", "Nutzer", "Prof. Dr.", Salutation.Mister, new DateTime(2000,2,20));
            ProposedUserContactDto proposedUserContactDto = new ProposedUserContactDto("proposed@user.de", "Straße", "1", "01234", "Stadt", Country.DE, "queo", "http://www.queo.de", "0123-456789", "0123-0123456", "0151-123456");
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(UserCreator.Create(), new DateTime(2017,01,01,01,01,01));
            ProposedUser user = new ProposedUser("proposedUser", proposedUserDataDto, proposedUserContactDto, entityCreatedDto);
            
            // when: 
            ProposedUserDao.Save(user);
            ProposedUserDao.FlushAndClear();
            ProposedUser actualUser = ProposedUserDao.Get(user.Id);
            // then:
            Assert.AreEqual(user, actualUser);
            actualUser.GetUserContactDto().Should().Be(proposedUserContactDto);
            actualUser.GetUserDataDto().Should().Be(proposedUserDataDto);
            actualUser.CreatedAt.Should().Be(entityCreatedDto.CreatedAt);
            actualUser.CreatedBy.Should().Be(entityCreatedDto.CreatedBy);
            actualUser.ChangedBy.Should().BeNull();
            actualUser.ChangedAt.Should().NotHaveValue();
        }

        [Test]
        public void TestUpdateProposedUser() {
            ProposedUser user = ProposedUserCreator.CreateProposedUser();
            ProposedUser userToUpdate = ProposedUserDao.Get(user.Id);

            ProposedUserDataDto proposedUserDataDto = ProposedUserCreator.CreateProposedUserDataDto("neuerVorname", "neuerNachname", Salutation.Mister, "titel", new DateTime(1990,09,09));

            ProposedUserContactDto proposedUserContactDto = ProposedUserCreator.CreateProposedUserContactDto("neue@email.com",
                "Nürnberger Eieruhren GmbH", "Nürnberger Ei", "0", "01067", "Dresden", Country.DE,
                "http://www.nuernberger-eier.de", "phone", "privat", "mobile", "fax");

            userToUpdate.Update(userToUpdate.UserName, proposedUserDataDto, proposedUserContactDto, new EntityChangedDto(UserCreator.Create(), DateTime.Now));

            ProposedUserDao.FlushAndClear();

            ProposedUser actualUser = ProposedUserDao.Get(userToUpdate.Id);

            DtoAssert.AreEqual(proposedUserDataDto, actualUser.GetUserDataDto());
            DtoAssert.AreEqual(proposedUserContactDto, actualUser.GetUserContactDto());
        }
    }
}