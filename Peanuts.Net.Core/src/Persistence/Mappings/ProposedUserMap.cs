using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class ProposedUserMap : EntityMap<ProposedUser> {
        protected ProposedUserMap() {
            /*Allgemeine Informationen*/
            Map(user => user.Salutation).Not.Nullable();
            Map(user => user.Title).Nullable();
            Map(user => user.UserName).Unique().Not.Nullable();
            Map(user => user.Email).Not.Nullable();
            Map(user => user.FirstName).Not.Nullable();
            Map(user => user.LastName).Not.Nullable();

            /*Kontaktdaten*/
            Map(user => user.Url).Nullable().Length(255);
            Map(user => user.Company).Nullable().Length(255);
            Map(user => user.Street).Not.Nullable().Length(255);
            Map(user => user.StreetNumber).Not.Nullable().Length(20);
            Map(user => user.PostalCode).Not.Nullable().Length(10);
            Map(user => user.City).Not.Nullable().Length(255);
            Map(user => user.Country, "CountryTwoLetterIsoCode").Not.Nullable().Length(2);

            Map(user => user.Phone).Nullable().Length(30);
            Map(user => user.PhonePrivate).Nullable().Length(30);
            Map(user => user.Mobile).Nullable().Length(30);

            /*Administrative Informationen*/
            Map(user => user.Birthday).Nullable().CustomSqlType("date");

            References(user => user.CreatedBy).Not.Nullable().ForeignKey("FK_CREATOR_OF_PROPOSED_USER");
            Map(user => user.CreatedAt).Not.Nullable();

            References(user => user.ChangedBy).Nullable().NotFound.Ignore();
            Map(user => user.ChangedAt).Nullable();
        }
    }
}