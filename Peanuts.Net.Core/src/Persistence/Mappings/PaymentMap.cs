using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class PaymentMap : EntityMap<Payment> {
        public PaymentMap() {
            References(payment => payment.Sender).ForeignKey("FK_PAYMENT_SENDER");
            References(payment => payment.Recipient).ForeignKey("FK_PAYMENT_RECIPIENT");

            Map(payment => payment.CreatedAt).Not.Nullable();
            References(payment => payment.CreatedBy).Nullable().ForeignKey("FK_PAYMENT_CREATED_BY");

            Map(payment => payment.Amount).Not.Nullable();
            Map(payment => payment.PaymentStatus).Not.Nullable().CustomSqlType("nvarchar(100)");
            Map(payment => payment.PaymentType).Not.Nullable().CustomSqlType("nvarchar(100)");
            Map(payment => payment.Text).Length(1000).Not.Nullable();

            Map(payment => payment.AcceptedAt).Nullable();
            References(payment => payment.AcceptedBy).Nullable().ForeignKey("FK_PAYMENT_ACCEPTED_BY");

            Map(payment => payment.DeclinedAt).Nullable();
            References(payment => payment.DeclinedBy).Nullable().ForeignKey("FK_PAYMENT_DECLINED_BY");
            Map(payment => payment.DeclineReason).Nullable().Length(1000);

            References(payment => payment.RequestRecipient).Not.Nullable().ForeignKey("FK_PAYMENT_REQUEST_RECIPIENT");
            References(payment => payment.RequestSender).Not.Nullable().ForeignKey("FK_PAYMENT_REQUEST_SENDER");
        }
    }
}