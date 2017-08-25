using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain {
    [TestFixture]
    public class UserTest {

        [Test]
        public void TestUserDataDto() {
            UserDataDto userDataDto = new UserDataDto(null, null, null, null);
            UserDataDto otherUserDataDto = new UserDataDto("Vorname", "Nachname", new DateTime(2000,01,01), "UserName");
            DtoAssert.TestEqualsAndGetHashCode(userDataDto, otherUserDataDto);
        }

        [Test]
        public void TestUserPermissionsDto() {

            UserPermissionDto userPermissionDto1 = new UserPermissionDto(new List<string>() {Roles.Administrator}, true);
            UserPermissionDto userPermissionDto2 = new UserPermissionDto(new List<string>() { Roles.Member }, false);

            DtoAssert.TestEqualsAndGetHashCode(userPermissionDto1, userPermissionDto2);
        }

        [Test]
        public void TestUserContactDto() {

            UserContactDto userContactDto1 = new UserContactDto("info@queo.com", "Straße-des-1.", "1", "01111", "Ort 1", Country.DE, "Unternehmen 1", "http://www.1.de", "0123/456789", "0123/4567890", "0151/123456");
            UserContactDto userContactDto2 = new UserContactDto("info@csharp.com", "Straße-des-2.", "2", "02222", "Ort 2", Country.AT, "Unternehmen 2", "http://www.2.de", "0223/456789", "0223/4567890", "0251/123456");

            DtoAssert.TestEqualsAndGetHashCode(userContactDto1, userContactDto2);
        }

        /// <summary>
        /// Testet das Hinzufügen, Beibehalten und Löschen von einem Nutzer zugeordneten Dokumenten.
        /// </summary>
        [Test]
        public void TestAddKeepAndRemoveDocumentFromUser() {

            //Given: Ein Nutzer
            Document document1 = new Document();
            Document document2 = new Document();
            Document document3 = new Document();
            User user = new User("1234567890", new UserContactDto(), new UserDataDto(), new UserPaymentDto(), new UserNotificationOptionsDto(), new UserPermissionDto(new List<string> { Roles.Administrator}, true), new List<Document>(), new EntityCreatedDto());


            //When: Dem Nutzer zwei Dokumente hinzugefügt werden
            //Then: Muss das korrekt funktionieren
            var newDocuments = new List<Document> {document1, document2};
            user.Update(user.PasswordHash, user.GetUserContactDto(), user.GetUserDataDto(), user.GetUserPaymentDto(), user.GetNotificationOptions(), user.GetUserPermissionDto(), newDocuments, new EntityChangedDto(user, DateTime.Now));
            user.Documents.ShouldBeEquivalentTo(newDocuments);


            //When: Dem Nutzer ein Dokumente hinzugefügt (document3), eines gelöscht (document1) und eines beibehalten (document2) werden soll
            //Then: Muss das korrekt funktionieren
            var updatedDocuments = new List<Document> {document2, document3};
            user.Update(user.PasswordHash, user.GetUserContactDto(), user.GetUserDataDto(), user.GetUserPaymentDto(), user.GetNotificationOptions(), user.GetUserPermissionDto(), updatedDocuments, new EntityChangedDto(user, DateTime.Now));
            user.Documents.ShouldBeEquivalentTo(updatedDocuments);

        }

    }
}