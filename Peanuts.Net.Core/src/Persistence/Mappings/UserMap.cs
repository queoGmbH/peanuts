using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    /// <summary>
    ///     Bildet das Mapping für einen <see cref="User" /> ab.
    /// </summary>
    public class UserMap : EntityMap<User> {
        protected UserMap() {
            /*Allgemeine Informationen*/
            Map(user => user.UserName).Unique().Not.Nullable();
            Map(user => user.Email);
            Map(user => user.PasswordHash);
            Map(user => user.FirstName);
            Map(user => user.LastName);
            Map(user => user.PasswordResetCode);

            /*Kontaktdaten*/
            Map(user => user.Url).Nullable().Length(255);
            Map(user => user.Company).Nullable().Length(255);
            Map(user => user.Street).Nullable().Length(255);
            Map(user => user.StreetNumber).Nullable().Length(20);
            Map(user => user.PostalCode).Nullable().Length(10);
            Map(user => user.City).Nullable().Length(255);
            Map(user => user.Country, "CountryTwoLetterIsoCode").Nullable().Length(2);

            Map(user => user.Phone).Nullable().Length(30);
            Map(user => user.PhonePrivate).Nullable().Length(30);
            Map(user => user.Mobile).Nullable().Length(30);

            Map(user => user.PayPalBusinessName).Nullable().Length(255);
            Map(user => user.AutoAcceptPayPalPayments).Not.Nullable();

            /*Benachrichtigungs-Einstellungen*/
            Map(user => user.NotifyMeAsCreditorOnPeanutDeleted).Not.Nullable();
            Map(user => user.NotifyMeAsCreditorOnPeanutRequirementsChanged).Not.Nullable();
            Map(user => user.NotifyMeAsParticipatorOnPeanutChanged).Not.Nullable();
            Map(user => user.NotifyMeAsCreditorOnDeclinedBills).Not.Nullable();
            Map(user => user.NotifyMeAsDebitorOnIncomingBills).Not.Nullable();
            Map(user => user.NotifyMeOnIncomingPayment).Not.Nullable();
            Map(user => user.NotifyMeAsCreditorOnSettleableBills).Not.Nullable();
            Map(user => user.NotifyMeOnPeanutInvitation).Not.Nullable();
            Map(user => user.SendMeWeeklySummaryAndForecast).Not.Nullable();


            /*Administrative Informationen*/
            Map(user => user.IsDeleted).Not.Nullable().Default("0");
            Map(user => user.IsEnabled).Not.Nullable();
            HasMany(user => user.Roles)
                    .Element("Role")
                    .Table("tblUserRoles")
                    .Cascade.AllDeleteOrphan()
                    .ForeignKeyConstraintName("FK_ROLE_TO_USER")
                    .Access.CamelCaseField(Prefix.Underscore);

            Map(user => user.Birthday).Nullable().CustomSqlType("date");

            References(user => user.CreatedBy).Nullable().NotFound.Ignore();
            Map(user => user.CreatedAt).Not.Nullable();

            References(user => user.ChangedBy).Nullable().NotFound.Ignore();
            Map(user => user.ChangedAt).Nullable();

            HasManyToMany(user => user.Documents)
                    .Access.CamelCaseField(Prefix.Underscore)
                    .Table("tblUserDocument")
                    .ForeignKeyConstraintNames("FK_USER_WITH_DOCUMENTS", "FK_USER_DOCUMENT");
        }
    }
}