using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    /// Schnittstelle, die einen Service beschreibt, der Nutzer über bestimmte Ereignisse benachrichtigt.
    /// </summary>
    public interface INotificationService {

        /// <summary>
        /// Sendet eine Benachrichtigung, über einen neuen Kommentar an einem Peanut.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="comment"></param>
        /// <param name="notificationOptions"></param>
        /// <param name="user"></param>
        void SendPeanutCommentNotification(Peanut peanut, string comment, PeanutUpdateNotificationOptions notificationOptions, User user);

        /// <summary>
        /// Sendet eine Benachrichtigung, über einen neuen Kommentar an einem Peanut.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="notifiedUser"></param>
        /// <param name="notificationOptions"></param>
        /// <param name="user"></param>
        void SendPeanutInvitationNotification(Peanut peanut, User notifiedUser, PeanutInvitationNotificationOptions notificationOptions, User user);

        /// <summary>
        /// Sendet eine Benachrichtigung, über die Änderung an einem Peanut.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="dtoBeforeUpdate"></param>
        /// <param name="requirementsBeforeUpdate"></param>
        /// <param name="updateComment"></param>
        /// <param name="notificationOptions"></param>
        /// <param name="user"></param>
        void SendPeanutUpdateNotification(Peanut peanut, PeanutDto dtoBeforeUpdate, IList<PeanutRequirement> requirementsBeforeUpdate, string updateComment, PeanutUpdateNotificationOptions notificationOptions, User user);

        /// <summary>
        /// Sendet eine Benachrichtigung, über die Änderung an einem Peanut.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="updateComment"></param>
        /// <param name="notificationOptions"></param>
        /// <param name="user"></param>
        void SendPeanutUpdateRequirementsNotification(Peanut peanut, string updateComment, PeanutUpdateRequirementsNotificationOptions notificationOptions, User user);

        /// <summary>
        /// Sendet eine Benachrichtigung an Administratoren, dass sich ein neuer Nutzer registriert und aktiviert hat.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="editLink"></param>
        void SendNotificationAboutUserActivation(User user, string editLink);

        /// <summary>
        /// Sendet eine Notification an alle Schuldner einer Rechnung mit dem Link zur Bestätigung
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="billUrl"></param>
        void SendBillReceivedNotification(Bill bill, string billUrl);

        /// <summary>
        /// Sendet eine Notification an alle Schuldner einer Rechnung mit dem Link zur Bestätigung
        /// </summary>
        /// <param name="bill">Die Rechnung, der alle Schuldner zugestimmt haben.</param>
        /// <param name="billUrl">Die Url zur Anzeige der Rechnung.</param> <summary>
        /// <param name="settleBillUrl">Die Url zum Formular, auf dem die Rechnung abgerechnet werden kann.</param>
        /// </summary>
        void SendBillAcceptedNotification(Bill bill, string billUrl, string settleBillUrl);

        /// <summary>
        /// Sendet eine Notification an alle Schuldner einer Rechnung mit dem Link zur Bestätigung
        /// </summary>
        /// <param name="bill">Die Rechnung, die abgelehnt wurde.</param>
        /// <param name="decliningDebitor">Der Schuldner, der die Zahlung der Rechnung abgelehnt hat.</param>
        /// <param name="billUrl">Die Url zur Anzeige der Rechnung.</param>
        void SendBillDeclinedNotification(Bill bill, BillUserGroupDebitor decliningDebitor, string billUrl);

        /// <summary>
        /// Sendet eine Benachrichtigung bei Änderung des Status eines Peanuts.
        /// </summary>
        /// <param name="peanut">Der Peanut, dessen Staus geändert wurde.</param>
        /// <param name="notificationOptions">Benachrichtigungs-Informationen.</param>
        /// <param name="user">Der Nutzer der den Status gesetzt hat.</param>
        void SendPeanutUpdateStateNotification(Peanut peanut, PeanutUpdateNotificationOptions notificationOptions, User user);

        /// Sendet eine Notification an den Nutzer mit der Information das sich ein Nutzer um eine Mitgliedschaft in einer Gruppe beworben hat.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="urlToUserGroup"></param>
        void SendRequestMembershipNotification(User user, string urlToUserGroup);

        /// <summary>
        /// Sendet eine Notification an den Nutzer das er zu einer Mitgliedschaft in einer Gruppe eingeladen wurde
        /// </summary>
        /// <param name="user"></param>
        /// <param name="allMembershipsUrl"></param>
        void SendUserGroupInvitationNotification(User user, string allMembershipsUrl);

        /// <summary>
        ///     Sendet eine Notification an denjenigen der eine Zahlung erhalten hat.
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="paymentUrl"></param>
        void SendPaymentReceivedNotification(Payment payment, string paymentUrl);

        /// <summary>
        ///     Sendet eine Notification an den Nutzer mit der Information das sich ein Nutzer um eine Mitgliedschaft in einer
        ///     Gruppe beworben hat.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userGroup"></param>
        /// <param name="urlToUserGroup"></param>
        void SendConfirmMembershipNotification(User user,UserGroup userGroup, string urlToUserGroup);
    }
}