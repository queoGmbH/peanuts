using System;
using System.Collections.Generic;
using System.IO;

using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

using FluentAssertions;

using Moq;

using NHibernate;

using NUnit.Framework;

using Spring.Context.Support;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    [TestFixture]
    public class UserServiceTest : ServiceBaseTest {
        public UserGroupCreator UserGroupCreator { get; set; }

        /// <summary>
        ///     Liefert oder setzt das UserUtil.
        /// </summary>
        public UserCreator UserCreator { get; set; }

        /// <summary>
        ///     Liefert oder setzt den UserService.
        /// </summary>
        public IUserService UserService { get; set; }

        public DocumentCreator DocumentCreator { get; set; }

        /// <summary>
        ///     Testet das Erstellen und das darauffolgende Laden eines Nutzers
        /// </summary>
        [Test]
        public void TestCreateAndLoadUser() {
            // when:
            UserContactDto updateUserContactDto = UserCreator.CreateUserContactDto("testi@test.com", "Teststraße", "?", "01234", "Teststadt", Country.DE, "Testunternehmen", "http://www.url.test", "phone", "privat", "mobile");
            UserDataDto updateUserDataDto = UserCreator.CreateUserDataDto("Vorname", "Nachname", new DateTime(1984,04,26), "UserName");
            UserPermissionDto updateUserPermissionDto = UserCreator.CreateUserPermissionDto(new List<string>() { Roles.Administrator }, true);
            UserPaymentDto userPaymentDto = UserCreator.CreateUserPaymentDto("PayPal", true);
            EntityCreatedDto creationDto = new EntityCreatedDto(UserCreator.Create(), new DateTime(2016, 12, 12, 15, 30, 0));
            
            User user = UserService.Create("passwordHash", updateUserContactDto, updateUserDataDto, userPaymentDto, updateUserPermissionDto, creationDto);

            User actualUser = UserService.GetByBusinessId(user.BusinessId);

            // then:
            Assert.AreEqual(user, actualUser);
            DtoAssert.AreEqual(updateUserContactDto, actualUser.GetUserContactDto());
            DtoAssert.AreEqual(updateUserDataDto, actualUser.GetUserDataDto());
            DtoAssert.AreEqual(updateUserPermissionDto, actualUser.GetUserPermissionDto());
            actualUser.CreatedBy.Should().Be(creationDto.CreatedBy);
            actualUser.CreatedAt.Should().Be(creationDto.CreatedAt);
        }

        /// <summary>
        ///     Testet das Löschen eines Nutzers
        /// </summary>
        [Test]
        public void TestDeleteUser() {
            // given: 
            User user = UserCreator.Create();
            // when:
            UserService.Delete(user);
            User actualUser = UserService.GetByBusinessId(user.BusinessId);
            // then: 
            Assert.IsNull(actualUser);
        }

        /// <summary>
        ///     Testet das Löschen eines Nutzers der mit Vorgängen verbunden ist.
        /// </summary>
        [Test]
        public void TestDeleteUserWithIssueReferences() {
            //Given: Ein Nutzer der mit Vorgängen verbunden ist.
            User userToDelete = UserCreator.Create();

            IUserService userService = new UserService();
            Mock<IUserDao> userDaoMock = new Mock<IUserDao>();
            userDaoMock.Setup(dao => dao.IsUserReferencesWithIssues(userToDelete)).Returns(true);
            userDaoMock.Setup(dao => dao.Delete(userToDelete));
            userService.UserDao = userDaoMock.Object;

            //When: Der Nutzer gelöscht werden soll
            userService.Delete(userToDelete);

            //Then: Darf er nicht gelöscht werden sondern nur archiviert
            userDaoMock.Verify(dao => dao.Delete(userToDelete), Times.Never);
            userToDelete.IsDeleted.Should().BeTrue();
        }

        /// <summary>
        ///     Testet das tatsächliche Löschen eines nicht mit Vorgängen verbundenen Nutzers.
        ///     TODO: Test sinnvoll benennen => Wie heißt die Vorgangs-Klasse?
        /// </summary>
        [Test]
        public void TestDeleteUserWithoutIssueReferences() {
            //Given: Ein Nutzer der nicht mit Vorgängen verbunden ist.
            User userToDelete = UserCreator.Create();
            User userCreatedByUserToDelete = UserCreator.Create(creationDto: new EntityCreatedDto(userToDelete, new DateTime(2017, 01, 01)));
            User userEditedByUserToDelete = UserCreator.Create(latestChangeDto: new EntityChangedDto(userToDelete, new DateTime(2017, 01, 01)));

            //When: Der Nutzer gelöscht wird
            UserService.Delete(userToDelete);
            UserService.UserDao.FlushAndClear();

            //Then: Darf er in der Anwendung nicht mehr vorhanden sein.
            Assert.Throws<ObjectNotFoundException>(() => UserService.GetById(userToDelete.Id));
        }

        /// <summary>
        ///     Testet das Finden eines Benutzers anhand seiner BusinessId
        /// </summary>
        [Test]
        public void TestFindUserByBusinessId() {
            // given: 
            User user = UserCreator.Create();
            // when:
            User actualUser = UserService.GetByBusinessId(user.BusinessId);
            // then: 
            Assert.AreEqual(user, actualUser);
        }

        /// <summary>
        ///     Testet das Finden eines Benutzers anhand seiner Email
        /// </summary>
        [Test]
        public void TestFindUserByEmail() {
            // given: 
            User user = UserCreator.Create();
            // when:
            User actualUser = UserService.FindByEmail(user.Email);
            // then: 
            Assert.AreEqual(user, actualUser);
        }

        /// <summary>
        ///     Testet das Finden eines Benutzers anhand seines Nutzernamens
        /// </summary>
        [Test]
        public void TestFindUserByUsername() {
            // given: 
            User user = UserCreator.Create();
            // when:
            User actualUser = UserService.FindByUserName(user.UserName);
            // then: 
            Assert.AreEqual(user, actualUser);
        }

        /// <summary>
        ///     Testet die Aktualisierung der Nutzerdaten
        /// </summary>
        [Test]
        public void TestUpdateUser() {
            // given: 

            Service.UserService userService = new UserService();
            userService.UserDao = ContextRegistry.GetContext().GetObject<IUserDao>();
            
            Document document1 = DocumentCreator.Create();
            Document document2 = DocumentCreator.Create();
            Document document3 = DocumentCreator.Create();
            User userToUpdate = UserCreator.Create(documents: new [] {document1, document2});


            UploadedFile uploadedFile = new UploadedFile(new FileInfo("C:\\Temp\\passbild.png"));

            Mock<IDocumentRepository> fileServiceMock = new Mock<IDocumentRepository>();
            fileServiceMock.Setup(s => s.Create(uploadedFile)).Returns(document3);
            userService.DocumentRepository = fileServiceMock.Object;

            // when:
            // UserLoginDto userLoginDto = UserUtil.CreateUserLoginDto("neuernutzer", "anderesPasswort", false);
            UserContactDto userContactDto = new UserContactDto("neue@email.com",
                "Straße",
                "1",
                "01234",
                "Stadt",
                Country.DE,
                "Unternehmen",
                "http://www.test.url",
                "phone",
                "privat",
                "mobile");
            const string NEW_USERNAME = "neuernutzer";
            const string NEW_PASSWORDHASH = "andererPasswortHash";
            UserDataDto userDataDto = new UserDataDto("neuerVorname", "neuerNachname", new DateTime(2000,02,02), NEW_USERNAME);
            UserPaymentDto userPaymentDto = new UserPaymentDto("PayPal", true);
            UserPermissionDto userPermissionDto = UserCreator.CreateUserPermissionDto(new List<string>() { Roles.Administrator }, false);
            EntityChangedDto changeDto = new EntityChangedDto(UserCreator.Create(), new DateTime(2017, 01, 01, 01, 02, 03));

            UserNotificationOptionsDto userNotificationsDto = UserNotificationOptionsDto.AllOff;
            userService.Update(userToUpdate,
                NEW_PASSWORDHASH,
                userContactDto,
                userDataDto,
                userPaymentDto,
                userNotificationsDto,
                userPermissionDto,
                new List<UploadedFile>() {uploadedFile }, 
                new List<Document> {document2}, 
                changeDto);
            userService.UserDao.FlushAndClear();

            User actualUser = UserService.GetById(userToUpdate.Id);
            // then: 
            Assert.AreEqual(NEW_USERNAME, actualUser.UserName, "Der Nutzername wurde nicht korrekt übernommen.");
            Assert.AreEqual(NEW_PASSWORDHASH, actualUser.PasswordHash, "Das Passwort wurde nicht korrekt übernommen.");
            DtoAssert.AreEqual(userContactDto, actualUser.GetUserContactDto());
            DtoAssert.AreEqual(userDataDto, actualUser.GetUserDataDto());
            DtoAssert.AreEqual(userNotificationsDto, actualUser.GetNotificationOptions());
            DtoAssert.AreEqual(userPermissionDto, actualUser.GetUserPermissionDto());

            actualUser.Documents.Should().BeEquivalentTo(document1, document3);

            actualUser.ChangedBy.Should().Be(changeDto.ChangedBy);
            actualUser.ChangedAt.Should().Be(changeDto.ChangedAt);

        }

        /// <summary>
        ///     Testet die Aktualisierung der Nutzerdaten, wenn keine Änderungen vorgenommen werden.
        /// </summary>
        [Test]
        public void TestUpdateUserNoChanges() {
            // given: 
            User user = UserCreator.Create(documents: new List<Document> {DocumentCreator.Create()});
            User changedBy = user.ChangedBy;
            DateTime? changedAt = user.ChangedAt;

            // when:
            UserService.Update(user, user.PasswordHash, user.GetUserContactDto(), user.GetUserDataDto(), user.GetUserPaymentDto(), user.GetNotificationOptions(), user.GetUserPermissionDto(), new List<UploadedFile>(), new List<Document>(), new EntityChangedDto(UserCreator.Create(), new DateTime(2017, 03, 03, 03, 03, 03)));
            UserService.UserDao.FlushAndClear();
            User actualUser = UserService.GetById(user.Id);

            // then: 
            actualUser.ChangedBy.Should().Be(changedBy);
            actualUser.ChangedAt.Should().Be(changedAt);
        }

        /// <summary>
        ///     Testet die Aktualisierung der Profildaten eines Nutzers
        /// </summary>
        [Test]
        public void TestUpdateUserProfile() {
            // given: 
            User userToUpdate = UserCreator.Create();
            UserContactDto userContactDto = new UserContactDto("neue@email.com", "Straße", "1", "01234", "Stadt", Country.DE, "Unternehmen", "http://www.test.url", "phone", "privat", "mobile");
            UserDataDto userDataDto = new UserDataDto("neuerVorname", "neuerNachname", new DateTime(1950,05,05), "UserName");
            UserPaymentDto userPaymentDto = new UserPaymentDto("PayPal", true);
            EntityChangedDto changeDto = new EntityChangedDto(UserCreator.Create(), new DateTime(2017, 01, 01, 01, 02, 03));
            UserNotificationOptionsDto notificationsDto = UserNotificationOptionsDto.AllOff;

            // when:
            UserService.Update(userToUpdate, userContactDto, userDataDto, userPaymentDto, notificationsDto, changeDto);
            UserService.UserDao.FlushAndClear();
            User actualUser = UserService.GetById(userToUpdate.Id);

            // then: 
            DtoAssert.AreEqual(userContactDto, actualUser.GetUserContactDto());
            DtoAssert.AreEqual(userDataDto, actualUser.GetUserDataDto());
            DtoAssert.AreEqual(notificationsDto, actualUser.GetNotificationOptions());

            actualUser.ChangedBy.Should().Be(changeDto.ChangedBy);
            actualUser.ChangedAt.Should().Be(changeDto.ChangedAt);
        }

        /// <summary>
        ///     Testet die Aktualisierung der Profildaten, wenn keine Änderungen vorgenommen werden.
        /// </summary>
        [Test]
        public void TestUpdateUserProfileNoChanges() {
            // given: 
            User user = UserCreator.Create();
            User changedBy = user.ChangedBy;
            DateTime? changedAt = user.ChangedAt;

            // when:
            UserService.Update(user, user.GetUserContactDto(), user.GetUserDataDto(), user.GetUserPaymentDto(), user.GetNotificationOptions(), new EntityChangedDto(UserCreator.Create(), new DateTime(2017, 03, 03, 03, 03, 03)));
            UserService.UserDao.FlushAndClear();
            User actualUser = UserService.GetById(user.Id);

            // then: 
            actualUser.ChangedBy.Should().Be(changedBy);
            actualUser.ChangedAt.Should().Be(changedAt);
        }
    }
}