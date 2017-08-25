using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto {
    /// <summary>
    ///     Dto mit Informationen und Einstellungen im Zusammenhang mit Zahlungen.
    /// </summary>
    [DtoFor(typeof(User))]
    public class UserPaymentDto {
        public UserPaymentDto() {
        }

        public UserPaymentDto(string payPalBusinessName, bool autoAcceptPayPalPayments) {
            PayPalBusinessName = payPalBusinessName;
            AutoAcceptPayPalPayments = autoAcceptPayPalPayments;
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob Zahlungen per PayPal automatisch akzeptiert werden oder nicht.
        /// </summary>
        public bool AutoAcceptPayPalPayments { get; set; }

        /// <summary>
        ///     Ruft den Namen des Nutzers bei PayPal ab oder legt diesen fest.
        /// </summary>
        public string PayPalBusinessName { get; set; }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((UserPaymentDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (PayPalBusinessName != null ? PayPalBusinessName.GetHashCode() : 0) * 397 ^ AutoAcceptPayPalPayments.GetHashCode();
            }
        }

        protected bool Equals(UserPaymentDto other) {
            return string.Equals(PayPalBusinessName, other.PayPalBusinessName) && AutoAcceptPayPalPayments == other.AutoAcceptPayPalPayments;
        }
    }
}