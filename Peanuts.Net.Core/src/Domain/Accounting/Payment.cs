using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Bildet eine Zahlung eines Nutzers an einen anderen Nutzer ab.
    /// </summary>
    public class Payment : Entity, IBillable {
        private readonly double _amount;
        private readonly DateTime _createdAt;
        private readonly User _createdBy;

        private readonly PaymentType _paymentType;

        private readonly Account _recipient;
        private readonly User _requestRecipient;

        private readonly User _requestSender;

        private readonly Account _sender;

        private readonly string _text;
        private DateTime? _acceptedAt;
        private User _acceptedBy;
        private DateTime? _declinedAt;
        private User _declinedBy;
        private string _declineReason;

        private PaymentStatus _paymentStatus = PaymentStatus.Pending;

        public Payment() {
        }

        public Payment(PaymentDto paymentDto, Account recipient, Account sender, User requestRecipient, User requestSender,
            EntityCreatedDto entityCreatedDto) {
            Require.NotNull(paymentDto, "paymentDto");
            Require.NotNull(recipient, "recipient");
            Require.NotNull(sender, "sender");
            Require.NotNull(entityCreatedDto, "entityCreatedDto");
            Require.NotNull(requestRecipient, "requestRecipient");
            Require.NotNull(requestSender, "requestSender");

            if (!recipient.Membership.UserGroup.Equals(sender.Membership.UserGroup)) {
                throw new InvalidOperationException(
                    $"Der Empfänger {recipient.Membership.User.DisplayName} muss in der gleichen Gruppe {sender.Membership.UserGroup.Name} wie der Sender {sender.Membership.User.DisplayName} sein. Ist aber in der Gruppe {recipient.Membership.UserGroup.Name}");
            }
            _amount = paymentDto.Amount;
            _text = paymentDto.Text;
            _paymentType = paymentDto.PaymentType;
            _recipient = recipient;
            _sender = sender;
            _createdBy = entityCreatedDto.CreatedBy;
            _createdAt = entityCreatedDto.CreatedAt;
            _requestRecipient = requestRecipient;
            _requestSender = requestSender;
        }

        /// <summary>
        ///     Ruft ab, wann die Zahlung akzeptiert wurde.
        /// </summary>
        public virtual DateTime? AcceptedAt {
            get { return _acceptedAt; }
        }

        /// <summary>
        ///     Ruft ab, von wem die Zahlung akzeptiert wurde.
        /// </summary>
        public virtual User AcceptedBy {
            get { return _acceptedBy; }
        }

        /// <summary>
        ///     Ruft den Zahlungsbetrag ab.
        /// </summary>
        public virtual double Amount {
            get { return _amount; }
        }

        public virtual DateTime BillingDate {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Ruft den Zeitpunkt der Erstellung des Nutzers ab.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return BillingDate; }
        }

        /// <summary>
        ///     Ruft ab, durch welchen Nutzer diese Zahlung erstellt wurde.
        ///     Hat sich der Nutzer registriert oder wurde er automatisiert erstellt ist die Eigenschaft NULL.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        /// <summary>
        ///     Ruft ab, wann die Zahlung abgelehnt wurde.
        /// </summary>
        public virtual DateTime? DeclinedAt {
            get { return _declinedAt; }
        }

        /// <summary>
        ///     Ruft ab, von wem die Zahlung abgelehnt wurde.
        /// </summary>
        public virtual User DeclinedBy {
            get { return _declinedBy; }
        }

        /// <summary>
        ///     Ruft einen Text ab, der begründet, warum die Zahlung abgelehnt wurde.
        /// </summary>
        public virtual string DeclineReason {
            get { return _declineReason; }
        }

        /// <summary>
        ///     Ruft den aktuellen Status der Zahlung ab.
        /// </summary>
        public virtual PaymentStatus PaymentStatus {
            get { return _paymentStatus; }
        }

        /// <summary>
        ///     Ruft ab, auf welche Art die Zahlung erfolgte.
        /// </summary>
        public virtual PaymentType PaymentType {
            get { return _paymentType; }
        }

        /// <summary>
        ///     Ruft den Nutzer ab, der das Geld erhält.
        ///     Empfänger
        /// </summary>
        public virtual Account Recipient {
            get { return _recipient; }
        }

        /// <summary>
        ///     Ruft den Nutzer ab, der die Zahlung bestätigen muss.
        /// </summary>
        /// <remarks>
        ///     Je nachdem, ob die Zahlung als Zahlungseingang oder als Zahlungsausgang erfasst wurde, unterscheidet sich der
        ///     Nutzer, der die Zahlung bestätigen muss.
        ///     Bei einer erhaltenen Zahlung, muss der Absender die Zahlung bestätigen,
        ///     bei einer getätigten Zahlung, muss der Empfänger die Zahlung bestätigen.
        ///     Nach der Bestätigung, kann die Zahlung abgerechnet und die entsprechenden Konten be- oder entlastet werden.
        /// </remarks>
        public virtual User RequestRecipient {
            get { return _requestRecipient; }
        }

        /// <summary>
        /// Ruft die Mitgliedschaft des Nutzers ab, der die Zahlung bestätigen muss.
        /// </summary>
        public virtual Account RequestRecipientAccount {
            get {
                if (_requestRecipient.Equals(_recipient.Membership.User)) {
                    /*Der Nutzer der die Zahlung bestätigen muss, ist der Empfänger*/
                    return _recipient.Membership.Account;
                } else if (_requestRecipient.Equals(_sender.Membership.User)) {
                    /*Der Nutzer der die Zahlung bestätigen muss, ist der Sender*/
                    return _sender.Membership.Account;
                } else {
                    /*Der Nutzer der die Zahlung bestätigen muss, ist weder Empfänger noch Absender*/
                    return null;
                }
            }
        }

        /// <summary>
        ///     Ruft den Nutzer ab, der die Zahlung erstellt hat und auf die Bestätigung wartet.
        /// </summary>
        /// <remarks>
        ///     Je nachdem, ob die Zahlung als Zahlungseingang oder als Zahlungsausgang erfasst wurde, unterscheidet sich der
        ///     Nutzer, der auf die Bestätigung der Zahlung wartet.
        ///     Bei einer erhaltenen Zahlung, wartet der Empfänger auf die Bestätigung,
        ///     bei einer getätigten Zahlung, wartet der Sender auf die Bestätigung.
        ///     Nach der Bestätigung, kann die Zahlung abgerechnet und die entsprechenden Konten be- oder entlastet werden.
        /// </remarks>
        public virtual User RequestSender {
            get { return _requestSender; }
        }

        /// <summary>
        ///     Ruft den Nutzer ab, der das Geld zahlt.
        ///     Auftraggeber
        /// </summary>
        public virtual Account Sender {
            get { return _sender; }
        }

        /// <summary>
        ///     Ruft einen Info-Text zur Zahlung ab.
        /// </summary>
        public virtual string Text {
            get { return _text; }
        }

        public virtual string DisplayName {
            get {
                return string.Format("{0:C} von {1} an {2}", _amount, _sender.Membership.User.DisplayName, _recipient.Membership.User.DisplayName);
            }
        }

        /// <summary>
        ///     Markiert die Zahlung als akzeptiert.
        /// </summary>
        /// <param name="entityChangedDto"></param>
        public virtual void Accept(EntityChangedDto entityChangedDto) {
            _paymentStatus = PaymentStatus.Accecpted;
            _acceptedBy = entityChangedDto.ChangedBy;
            _acceptedAt = entityChangedDto.ChangedAt;
        }

        /// <summary>
        ///     Markiert die Zahlung als abgelehnt.
        /// </summary>
        /// <param name="declineReason">Warum wird die Zahlung abgelehnt?</param>
        /// <param name="entityChangedDto">Wann und von wem wurde die Zahlung abgelehnt.</param>
        public virtual void Decline(string declineReason, EntityChangedDto entityChangedDto) {
            Require.NotNull(entityChangedDto, "entityChangedDto");
            Require.NotNullOrWhiteSpace(declineReason, "declineReason");

            _paymentStatus = PaymentStatus.Declined;
            _declineReason = declineReason;

            _declinedBy = entityChangedDto.ChangedBy;
            _declinedAt = entityChangedDto.ChangedAt;
        }

      
    }
}