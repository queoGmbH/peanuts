using System;
using System.Collections.Generic;
using System.Linq;
using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders;
using FluentAssertions;

using NHibernate;

using NUnit.Framework;

using Spring.Dao;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    [TestFixture]
    public class UserDaoTest : PersistenceBaseTest {
        /// <summary>
        ///     Liefert oder setzt den UserDao.
        /// </summary>
        public UserDao UserDao { get; set; }

        /// <summary>
        ///     Liefert oder setzt das UserUtil.
        /// </summary>
        public UserCreator UserCreator { get; set; }

        public UserGroupCreator UserGroupCreator { get; set; }

        public DocumentCreator DocumentCreator { get; set; }

        /// <summary>
        ///     Testet Das Löschen eines gespeicherten Nutzers
        /// </summary>
        [Test]
        public void TestDeleteUser() {
            // given:
            User user = UserCreator.Create();
            // when:
            User actualUser = UserDao.Get(user.Id);
            UserDao.Delete(actualUser);
            UserDao.FlushAndClear();

            // then: Exception
            Assert.Throws<ObjectNotFoundException>(() => UserDao.Get(user.Id));
        }

        /// <summary>
        ///     Testet das Suchen nach einem Nutzer anhand seiner E-Mail-Adresse
        /// </summary>
        [Test]
        public void TestFindByEmail() {
            //Given: Mehrere Nutzer mit unterschiedlichen E-Mail-Adressen
            const string SEARCH_TERM = "web";
            User emailStartsWith = UserCreator.Create(email: SEARCH_TERM + "@gmx.net");
            User emailEndsWith = UserCreator.Create(email: "info@online." + SEARCH_TERM);
            User emailContains = UserCreator.Create(email: "irgendwas@" + SEARCH_TERM + ".de");
            User emailDoesNotContain = UserCreator.Create(email: "irgendwas@gmx.net");

            //When: Nach Nutzern mit einer bestimmten Zeichenfolge in ihrem Nutzernamen gesucht wird 
            IPage<User> foundUsers = UserDao.FindUser(PageRequest.All, SEARCH_TERM);

            //Then: Dürfen nur Nutzer gefunden werden die diese Zeichenfolge in ihrem Nutzernamen haben
            foundUsers.ShouldAllBeEquivalentTo(new[] { emailContains, emailEndsWith, emailStartsWith });
        }

        /// <summary>
        ///     Testet das Suchen nach einem Nutzer anhand seines Vornamens
        /// </summary>
        [Test]
        public void TestFindByFirstname() {
            //Given: Mehrere Nutzer mit unterschiedlichen Vornamen
            const string SEARCH_TERM = "xyz";
            User userFirstnameStartsWith = UserCreator.Create(firstname: SEARCH_TERM + "irgendwas");
            User userFirstnameEndsWith = UserCreator.Create(firstname: "irgendwas" + SEARCH_TERM);
            User userFirstnameContains = UserCreator.Create(firstname: "irgend" + SEARCH_TERM + "was");
            User userFirstnameDoesNotContain = UserCreator.Create(firstname: "irgendwas");

            //When: Nach Nutzern mit einer bestimmten Zeichenfolge in ihrem Vornamen gesucht wird 
            IPage<User> foundUsers = UserDao.FindUser(PageRequest.All, SEARCH_TERM);

            //Then: Dürfen nur Nutzer gefunden werden die diese Zeichenfolge in ihrem Vornamen haben
            foundUsers.ShouldAllBeEquivalentTo(new[] { userFirstnameContains, userFirstnameEndsWith, userFirstnameStartsWith });
        }

        /// <summary>
        ///     Testet das Suchen nach Nutzern anhand ihrer Vor- oder Nachnamens
        /// </summary>
        [Test]
        public void TestFindByFirstOrLastname() {
            //Given: Mehrere Nutzer mit unterschiedlichen Vor- und Nachnamen
            const string SEARCH_TERM = "xyz";
            User userLastnameStartsWith = UserCreator.Create(lastname: SEARCH_TERM + "irgendwas");
            User userLastnameEndsWith = UserCreator.Create(lastname: "irgendwas" + SEARCH_TERM);
            User userLastnameContains = UserCreator.Create(lastname: "irgend" + SEARCH_TERM + "was");
            User userFirstnameStartsWith = UserCreator.Create(lastname: SEARCH_TERM + "irgendwas");
            User userFirstnameEndsWith = UserCreator.Create(lastname: "irgendwas" + SEARCH_TERM);
            User userFirstnameContains = UserCreator.Create(lastname: "irgend" + SEARCH_TERM + "was");
            User userFirstAndLastnameDoesNotContain = UserCreator.Create(firstname: "irgend", lastname: "was");

            //When: Nach Nutzern mit einer bestimmten Zeichenfolge in ihrem Vor- oder Nachnamen gesucht wird 
            IPage<User> foundUsers = UserDao.FindUser(PageRequest.All, SEARCH_TERM);

            //Then: Dürfen nur Nutzer gefunden werden die diese Zeichenfolge entweder in ihrem Vor- oder ihrem Nachnamen haben
            foundUsers.ShouldAllBeEquivalentTo(new[] {
                userLastnameContains, userLastnameEndsWith, userLastnameStartsWith, userFirstnameContains, userFirstnameEndsWith,
                userFirstnameStartsWith
            });
        }

        /// <summary>
        ///     Testet das Suchen nach einem Nutzer anhand seines Nachnamens
        /// </summary>
        [Test]
        public void TestFindByLastname() {
            //Given: Mehrere Nutzer mit unterschiedlichen Nachnamen
            const string SEARCH_TERM = "xyz";
            User userFirstnameStartsWith = UserCreator.Create(lastname: SEARCH_TERM + "irgendwas");
            User userFirstnameEndsWith = UserCreator.Create(lastname: "irgendwas" + SEARCH_TERM);
            User userFirstnameContains = UserCreator.Create(lastname: "irgend" + SEARCH_TERM + "was");
            User userFirstnameDoesNotContain = UserCreator.Create(lastname: "irgendwas");

            //When: Nach Nutzern mit einer bestimmten Zeichenfolge in ihrem Nachnamen gesucht wird 
            IPage<User> foundUsers = UserDao.FindUser(PageRequest.All, SEARCH_TERM);

            //Then: Dürfen nur Nutzer gefunden werden die diese Zeichenfolge in ihrem Nachnamen haben
            foundUsers.ShouldAllBeEquivalentTo(new[] { userFirstnameContains, userFirstnameEndsWith, userFirstnameStartsWith });
        }

        /// <summary>
        ///     Prüft ob alle Nutzer einer bestimmten Rolle gefunden werden.
        /// </summary>
        [Test]
        public void TestFindByRole() {
            //Given: GIVEN
            User user1 = UserCreator.Create(roles: new List<string>() { Roles.Administrator });
            User user2 = UserCreator.Create(roles: new List<string>() { Roles.Administrator, Roles.Member });
            User user3 = UserCreator.Create(roles: new List<string>() { Roles.Member });

            //When: WHEN
            IList<User> users = UserDao.FindByRole(Roles.Administrator);
            
            //Then: THEN
            Assert.IsTrue(users.Contains(user1));
            Assert.IsTrue(users.Contains(user2));
            Assert.IsFalse(users.Contains(user3));
        }

        /// <summary>
        ///     Testet das Suchen nach einem Nutzer anhand seines Nutzernamens
        /// </summary>
        [Test]
        public void TestFindByUsername() {
            //Given: Mehrere Nutzer mit unterschiedlichen Nutzernamen
            const string SEARCH_TERM = "xyz";
            User usernameStartsWith = UserCreator.Create(SEARCH_TERM + "irgendwas");
            User usernameEndsWith = UserCreator.Create("irgendwas" + SEARCH_TERM);
            User usernameContains = UserCreator.Create("irgend" + SEARCH_TERM + "was");
            User usernameDoesNotContain = UserCreator.Create("irgendwas");

            //When: Nach Nutzern mit einer bestimmten Zeichenfolge in ihrem Nutzernamen gesucht wird 
            IPage<User> foundUsers = UserDao.FindUser(PageRequest.All, SEARCH_TERM);

            //Then: Dürfen nur Nutzer gefunden werden die diese Zeichenfolge in ihrem Nutzernamen haben
            foundUsers.ShouldAllBeEquivalentTo(new[] { usernameContains, usernameEndsWith, usernameStartsWith });
        }

        /// <summary>
        ///     Testet Das Löschen eines gespeicherten Nutzers
        /// </summary>
        [Test]
        public void TestGetUserByBuisnessId() {
            // given:
            User user = UserCreator.Create();
            // when:
            User actualUser = UserDao.GetByBusinessId(user.BusinessId);
            // then:
            Assert.AreEqual(user, actualUser);
        }

        /// <summary>
        ///     Testet Das Löschen eines gespeicherten Nutzers
        /// </summary>
        [Test]
        public void TestGetUserByEmail() {
            // given:
            User user = UserCreator.Create();
            // when:
            User actualUser = UserDao.FindByEmail(user.Email);
            // then:
            Assert.AreEqual(user, actualUser);
        }

        /// <summary>
        ///     Testet Das Löschen eines gespeicherten Nutzers
        /// </summary>
        [Test]
        public void TestGetUserByUserName() {
            // given:
            User user = UserCreator.Create();
            // when:
            User actualUser = UserDao.FindByUserName(user.UserName);
            // then:
            Assert.AreEqual(user, actualUser);
        }

        /// <summary>
        ///     Testet das Speichern und Laden eines Users
        /// </summary>
        [Test]
        public void TestSaveAndLoad() {
            // given:
            User user = UserCreator.Create();
            // when:
            User actualUser = UserDao.Get(user.Id);
            // then:
            Assert.AreEqual(user, actualUser);
        }

        [Test]
        public void TestTwoUsersWithSameUsername() {
            const string DUPLICATE_USERNAME = "doppelterName";
            UserCreator.Create(DUPLICATE_USERNAME);
            User userWithSameUsername = UserCreator.Create(DUPLICATE_USERNAME, persist: false);

            Assert.Throws<DataIntegrityViolationException>(() => UserDao.Save(userWithSameUsername));
        }

        /// <summary>
        ///     Testet das Updaten eines Users anhand eines UserLoginDtos
        /// </summary>
        [Test]
        public void TestUpdateUserAllProperties() {
            // given:
            User user = UserCreator.Create();
            User userToUpdate = UserDao.Get(user.Id);
            UserContactDto userContactDto = UserCreator.CreateUserContactDto("neue@email.com",
                "Nürnberger Ei",
                "0",
                "01067",
                "Dresden",
                Country.DE,
                "Nürnberger Eieruhren GmbH",
                "http://www.nuernberger-eier.de",
                "phone",
                "privat",
                "mobile");
            string username = "newUsername";
            UserDataDto userDataDto = UserCreator.CreateUserDataDto("neuerVorname", "neuerNachname", null, username);
            User changedBy = UserCreator.Create();
            EntityChangedDto entityChangedDto = new EntityChangedDto(changedBy, new DateTime(2017, 01, 01, 01, 01, 01));
            UserPermissionDto userPermissionDto = new UserPermissionDto(new List<string> { Roles.Administrator }, true);
            UserNotificationOptionsDto notificationsDto = UserNotificationOptionsDto.AllOn;
            string passwordHash = "newPasswordhash";
            List<Document> documents = new List<Document> { DocumentCreator.Create() };
            UserPaymentDto userPaymentDto = new UserPaymentDto("", false);

            // when:
            userToUpdate.Update(passwordHash,
                userContactDto,
                userDataDto,
                userPaymentDto,
                notificationsDto,
                userPermissionDto,
                documents,
                entityChangedDto);
            UserDao.FlushAndClear();
            User actualUser = UserDao.Get(userToUpdate.Id);

            // then:
            DtoAssert.AreEqual(userContactDto, actualUser.GetUserContactDto());
            DtoAssert.AreEqual(userDataDto, actualUser.GetUserDataDto());
            DtoAssert.AreEqual(notificationsDto, actualUser.GetNotificationOptions());
            DtoAssert.AreEqual(userPermissionDto, actualUser.GetUserPermissionDto());

            actualUser.ChangedBy.Should().Be(entityChangedDto.ChangedBy);
            actualUser.ChangedAt.Should().Be(entityChangedDto.ChangedAt);

            actualUser.UserName.Should().Be(username);
            actualUser.PasswordHash.Should().Be(passwordHash);

            actualUser.Documents.ShouldBeEquivalentTo(documents);
        }

        /// <summary>
        ///     Testet das Updaten eines Users anhand eines UserLoginDtos
        /// </summary>
        [Test]
        public void TestUpdateUserProfileProperties() {
            // given:
            User user = UserCreator.Create();
            User userToUpdate = UserDao.Get(user.Id);
            UserContactDto userContactDto = UserCreator.CreateUserContactDto("neue@email.com",
                "Nürnberger Ei",
                "0",
                "01067",
                "Dresden",
                Country.DE,
                "Nürnberger Eieruhren GmbH",
                "http://www.nuernberger-eier.de",
                "phone",
                "privat",
                "mobile");
            UserDataDto userDataDto = UserCreator.CreateUserDataDto("neuerVorname", "neuerNachname", new DateTime(1990, 01, 03), "UserName");
            UserPaymentDto userPaymentDto = new UserPaymentDto("paypal", true);
            UserNotificationOptionsDto notificationsDto = UserNotificationOptionsDto.AllOn;
            User changedBy = UserCreator.Create();
            EntityChangedDto entityChangedDto = new EntityChangedDto(changedBy, new DateTime(2017, 01, 01, 01, 01, 01));

            // when:
            userToUpdate.Update(userContactDto, userDataDto, userPaymentDto, notificationsDto, entityChangedDto);
            UserDao.FlushAndClear();
            User actualUser = UserDao.Get(userToUpdate.Id);

            // then:
            DtoAssert.AreEqual(userContactDto, actualUser.GetUserContactDto());
            DtoAssert.AreEqual(userDataDto, actualUser.GetUserDataDto());

            actualUser.ChangedBy.Should().Be(entityChangedDto.ChangedBy);
            actualUser.ChangedAt.Should().Be(entityChangedDto.ChangedAt);
        }
    }
}