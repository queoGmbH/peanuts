using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils {
    public class ProposedUserCreator : EntityCreator {
        private const string CITY_CONST = "Dresden";
        private const string COMPANY_CONST = "queo";
        private const Country COUNTRY_CONST = Country.DE;
        private const string NACHNAME_CONST = "Nachname";
        private const string POSTALCODE_CONST = "01159";
        private const string STREET_CONST = "Tharandter Str.";
        private const string STREET_NUMBER_CONST = "13";
        private const string URL_CONST = "http://www.queo.biz";
        private const string VORNAME_CONST = "Vorname";

        public IProposedUserDao ProposedUserDao { private get; set; }

        public UserCreator UserCreator { get; set; }

        public ProposedUser Create(string username, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto, EntityCreatedDto entityChangedDto, bool persist = true) {
            ProposedUser user = new ProposedUser(username,
                proposedUserDataDto,
                proposedUserContactDto,
                entityChangedDto);

            if (persist) {
                ProposedUserDao.Save(user);
                ProposedUserDao.Flush();
            }
            return user;
        }

        public ProposedUser CreateProposedUser(string username = null, string email = "Testi@test.com",
            string lastname = NACHNAME_CONST, Salutation salutation = Salutation.Mister, string title = null,
            string firstname = VORNAME_CONST, DateTime? birthday = null,
            string street = STREET_CONST, string streetNumber = STREET_NUMBER_CONST,
            string postalCode = POSTALCODE_CONST, string city = CITY_CONST,
            Country countryTwoLetterIsoCode = COUNTRY_CONST, string company = COMPANY_CONST, string url = URL_CONST,
            string phone = null, string phonePrivate = null, string mobile = null, string fax = null,
            User createdBy = null, bool persist = true) {
            if (username == null) {
                username = GetRandomString(10);
            }
            if (firstname == VORNAME_CONST) {
                firstname = firstname + GetRandomString(4);
            }

            ProposedUserDataDto proposedUserDataDto = CreateProposedUserDataDto(firstname, lastname, salutation, title, birthday);
            ProposedUserContactDto proposedUserContactDto = CreateProposedUserContactDto(email,
                company,
                street,
                streetNumber,
                postalCode,
                city,
                countryTwoLetterIsoCode,
                url,
                phone,
                phonePrivate,
                mobile,
                fax);

            if (createdBy == null) {
                createdBy = UserCreator.Create();
            }

            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(createdBy, DateTime.Now);

            ProposedUser user = Create(username,
                proposedUserDataDto,
                proposedUserContactDto,
                entityCreatedDto,
                persist);

            return user;
        }

        
        public ProposedUserContactDto CreateProposedUserContactDto(string email, string company, string street, string streetNumber,
            string postalCode, string city, Country countryTwoLetterIsoCode, string url, string phone, string phonePrivate, string mobile,
            string fax) {
            return new ProposedUserContactDto(email,
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
        }

        public ProposedUserDataDto CreateProposedUserDataDto(string firstname, string lastname, Salutation salutation, string title, DateTime? birthday) {
            return new ProposedUserDataDto(firstname, lastname, title, salutation, birthday);
        }
    }
}