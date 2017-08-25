using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    /// <summary>
    ///     Dto, welches die Einstellungen für E-Mail-Benachrichtigungen enthält.
    /// </summary>
    [DtoFor(typeof(User))]
    public class UserNotificationOptionsDto {
        public UserNotificationOptionsDto() {
        }

        public UserNotificationOptionsDto(bool notifyMeAsCreditorOnPeanutDeleted, bool notifyMeAsCreditorOnPeanutRequirementsChanged,
            bool notifyMeAsParticipatorOnPeanutChanged, bool notifyMeAsCreditorOnDeclinedBills, bool notifyMeAsDebitorOnIncomingBills, bool notifyMeOnIncomingPayment,
            bool notifyMeAsCreditorOnSettleableBills, bool sendMeWeeklySummaryAndForecast, bool notifyMeOnPeanutInvitation) {
            NotifyMeAsCreditorOnPeanutDeleted = notifyMeAsCreditorOnPeanutDeleted;
            NotifyMeAsCreditorOnPeanutRequirementsChanged = notifyMeAsCreditorOnPeanutRequirementsChanged;
            NotifyMeAsParticipatorOnPeanutChanged = notifyMeAsParticipatorOnPeanutChanged;
            NotifyMeAsCreditorOnDeclinedBills = notifyMeAsCreditorOnDeclinedBills;
            NotifyMeAsDebitorOnIncomingBills = notifyMeAsDebitorOnIncomingBills;
            NotifyMeOnIncomingPayment = notifyMeOnIncomingPayment;
            NotifyMeAsCreditorOnSettleableBills = notifyMeAsCreditorOnSettleableBills;
            SendMeWeeklySummaryAndForecast = sendMeWeeklySummaryAndForecast;
            NotifyMeOnPeanutInvitation = notifyMeOnPeanutInvitation;
        }

        public static UserNotificationOptionsDto AllOff {
            get { return new UserNotificationOptionsDto(false, false, false, false, false, false, false, false, false); }
        }

        public static UserNotificationOptionsDto AllOn {
            get { return new UserNotificationOptionsDto(true, true, true, true, true, true, true, true, true); }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn ein Peanut an dem er teilnimmt
        ///     gelöscht wurde.
        /// </summary>
        public bool NotifyMeAsCreditorOnPeanutDeleted { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er als Kreditor an einem Peanut
        ///     teilnimmt und die Anforderungsliste geändert wurde.
        /// </summary>
        public bool NotifyMeAsCreditorOnPeanutRequirementsChanged { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn ein Peanut an dem er teilnimmt
        ///     geändert wurde.
        /// </summary>
        public bool NotifyMeAsParticipatorOnPeanutChanged { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer als Kreditor eine Benachrichtigung erhalten möchte, wenn ein Schuldner die
        ///     Rechnung abgelehnt hat.
        /// </summary>
        public bool NotifyMeAsCreditorOnDeclinedBills { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er eine neue Rechnung erhalten
        ///     hat.
        /// </summary>
        public bool NotifyMeAsDebitorOnIncomingBills { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er den Eingang oder Ausgang einer
        ///     Bezahlung bestätigen muss.
        /// </summary>
        public bool NotifyMeOnIncomingPayment { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er zu einem Peanut eingeladen
        ///     wurde.
        /// </summary>
        public bool NotifyMeOnPeanutInvitation { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer als Kreditor eine Benachrichtigung erhalten möchte, wenn eine Rechnung von
        ///     allen Schuldnern  akzeptiert wurde und abgerechnet werden kann.
        /// </summary>
        public bool NotifyMeAsCreditorOnSettleableBills { get; set; }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer wöchentliche eine Zusammenfassung der vergangenen Woche sowie eine Vorschau
        ///     auf die nächste Woche erhalten möchte.
        /// </summary>
        public bool SendMeWeeklySummaryAndForecast { get; set; }

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
            return Equals((UserNotificationOptionsDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = NotifyMeAsCreditorOnPeanutDeleted.GetHashCode();
                hashCode = hashCode * 397 ^ NotifyMeAsCreditorOnPeanutRequirementsChanged.GetHashCode();
                hashCode = hashCode * 397 ^ NotifyMeAsParticipatorOnPeanutChanged.GetHashCode();
                hashCode = hashCode * 397 ^ NotifyMeOnPeanutInvitation.GetHashCode();
                hashCode = hashCode * 397 ^ NotifyMeAsCreditorOnDeclinedBills.GetHashCode();
                hashCode = hashCode * 397 ^ NotifyMeAsDebitorOnIncomingBills.GetHashCode();
                hashCode = hashCode * 397 ^ NotifyMeOnIncomingPayment.GetHashCode();
                hashCode = hashCode * 397 ^ NotifyMeAsCreditorOnSettleableBills.GetHashCode();
                hashCode = hashCode * 397 ^ SendMeWeeklySummaryAndForecast.GetHashCode();
                return hashCode;
            }
        }

        protected bool Equals(UserNotificationOptionsDto other) {
            return NotifyMeAsCreditorOnPeanutDeleted == other.NotifyMeAsCreditorOnPeanutDeleted
                   && NotifyMeAsCreditorOnPeanutRequirementsChanged == other.NotifyMeAsCreditorOnPeanutRequirementsChanged
                   && NotifyMeAsParticipatorOnPeanutChanged == other.NotifyMeAsParticipatorOnPeanutChanged
                   && NotifyMeOnPeanutInvitation == other.NotifyMeOnPeanutInvitation && NotifyMeAsCreditorOnDeclinedBills == other.NotifyMeAsCreditorOnDeclinedBills
                   && NotifyMeAsDebitorOnIncomingBills == other.NotifyMeAsDebitorOnIncomingBills && NotifyMeOnIncomingPayment == other.NotifyMeOnIncomingPayment
                   && NotifyMeAsCreditorOnSettleableBills == other.NotifyMeAsCreditorOnSettleableBills
                   && SendMeWeeklySummaryAndForecast == other.SendMeWeeklySummaryAndForecast;
        }
    }
}