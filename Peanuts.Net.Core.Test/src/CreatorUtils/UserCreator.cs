using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils {
    /// <summary>
    ///     Hilfsklasse zur Erstellung von Nutzer-Objekten und zugehörigen Objekten für Testfälle.
    /// </summary>
    public class UserCreator : EntityCreator {
        private const string STREET_CONST = "Tharandter Str.";
        private const string STREET_NUMBER_CONST = "13";
        private const string POSTALCODE_CONST = "01159";
        private const string CITY_CONST = "Dresden";
        private const Country COUNTRY_CONST = Country.DE;
        private const string COMPANY_CONST = "queo";
        private const string URL_CONST = "http://www.queo.biz";
        private const string NACHNAME_CONST = "Nachname";
        private const string VORNAME_CONST = "Vorname";

        /// <summary>
        ///     Legt den Dao für das Persistieren von Nutzern fest.
        /// </summary>
        public IUserDao UserDao { private get; set; }

        /// <summary>
        ///     Erzeugt einen neuen Nutzer
        /// </summary>
        /// <returns></returns>
        public User Create(string username = null, string email = "Testi@test.com", string lastname = NACHNAME_CONST,
            string firstname = VORNAME_CONST, string passwordHash = null,
            string street = STREET_CONST, string streetNumber = STREET_NUMBER_CONST, string postalCode = POSTALCODE_CONST, string city = CITY_CONST,
            Country countryTwoLetterIsoCode = COUNTRY_CONST, string company = COMPANY_CONST, string url = URL_CONST,
            string phone = null, string phonePrivate = null, string mobile = null, string fax = null,
            string payPalBusinessName  = null, bool autoAcceptPayPalPayments = true, 
            EntityCreatedDto creationDto = null, EntityChangedDto latestChangeDto = null,
            IList<string> roles = null, bool isEnabled = true, IList<Document> documents = null, DateTime? birthday = null, bool persist = true) {
            if (username == null) {
                username = GetRandomString(10);
            }
            if (firstname == VORNAME_CONST) {
                firstname = firstname + GetRandomString(4);
            }

            if (roles == null) {
                roles = new List<string> { Roles.Member };
            }
            if (documents == null) {
                documents = new List<Document>();
            }

            UserContactDto userContactDto = CreateUserContactDto(email,
                street,
                streetNumber,
                postalCode,
                city,
                countryTwoLetterIsoCode,
                company,
                url,
                phone,
                phonePrivate,
                mobile);
            UserDataDto userDataDto = CreateUserDataDto(firstname, lastname, birthday, username);
            UserPermissionDto userPermissionDto = CreateUserPermissionDto(roles, isEnabled);
            UserPaymentDto userPaymentDto = CreateUserPaymentDto(payPalBusinessName, autoAcceptPayPalPayments);
            UserNotificationOptionsDto userNotifications = UserNotificationOptionsDto.AllOn;
            if (creationDto == null) {
                creationDto = new EntityCreatedDto(null, DateTime.Now.Date);
            }

            User user = Create(username, passwordHash, userDataDto, userContactDto, userPaymentDto, userNotifications, userPermissionDto, creationDto, latestChangeDto, documents, persist);

            return user;
        }

        public UserPaymentDto CreateUserPaymentDto(string payPalBusinessName, bool autoAcceptPayPalPayments) {
            return new UserPaymentDto(payPalBusinessName, autoAcceptPayPalPayments);
        }

        /// <summary>
        ///     Erstellt einen zufälligen Benutzer und gibt diesen zurück wenn keine Dtos übergeben werden
        /// </summary>
        /// <returns></returns>
        public User Create(string username, string passwordHash, UserDataDto userDataDto,
            UserContactDto userContactDto, UserPaymentDto userPaymentDto, UserNotificationOptionsDto userNotifications,
            UserPermissionDto userPermissionDto,
            EntityCreatedDto entityCreatedDto, EntityChangedDto entityChangedDto, IList<Document> documents, bool persist = true) {
            User user = new User(passwordHash,
                userContactDto,
                userDataDto,
                userPaymentDto,
                userNotifications,
                userPermissionDto,
                documents,
                entityCreatedDto);
            if (entityChangedDto != null) {
                user.Update(userContactDto, userDataDto, userPaymentDto, userNotifications, entityChangedDto);
            }

            if (persist) {
                UserDao.Save(user);
                UserDao.Flush();
            }
            return user;
        }

        public UserContactDto CreateUserContactDto(string email, string street, string streetNumber, string postalCode, string city,
            Country countryTwoLetterIsoCode, string company, string url, string phone, string phonePrivate, string mobile) {
            UserContactDto userContactDto = new UserContactDto(email,
                street,
                streetNumber,
                postalCode,
                city,
                countryTwoLetterIsoCode,
                company,
                url,
                phone,
                phonePrivate,
                mobile);
            return userContactDto;
        }

        public UserDataDto CreateUserDataDto(string firstname, string lastname, DateTime? birthday, string userName) {
            UserDataDto userDataDto = new UserDataDto(firstname, lastname, birthday, userName);
            return userDataDto;
        }

        public UserPermissionDto CreateUserPermissionDto(IList<string> roles, bool isEnabled) {
            UserPermissionDto userPermissionDto = new UserPermissionDto(roles, isEnabled);
            return userPermissionDto;
        }
    }
}