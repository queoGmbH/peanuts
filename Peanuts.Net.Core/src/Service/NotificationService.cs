using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public class NotificationService : INotificationService {
        public IEmailService EmailService { private get; set; }

        /// <summary>
        ///     Liefert oder setzt den UserService
        /// </summary>
        public IUserService UserService { get; set; }

        public void SendBillAcceptedNotification(Bill bill, string billUrl, string settleBillUrl) {
            Require.NotNull(bill, "bill");

            if (bill.Creditor.User.NotifyMeAsCreditorOnSettleableBills) {
                string userEmail = bill.Creditor.User.Email;
                ModelMap modelMap = new ModelMap();
                modelMap.Add("creditor", bill.Creditor.User);
                modelMap.Add("bill", bill);
                modelMap.Add("billUrl", billUrl);
                modelMap.Add("settleBillUrl", settleBillUrl);
                MailMessage mailMessage = EmailService.CreateMailMessage(userEmail, modelMap, "BillAccepted");
                EmailService.SendMessage(mailMessage);
            }
        }

        public void SendBillDeclinedNotification(Bill bill, BillUserGroupDebitor decliningDebitor, string billUrl) {
            Require.NotNull(bill, "bill");
            Require.NotNull(decliningDebitor, "decliningDebitor");

            if (bill.Creditor.User.NotifyMeAsCreditorOnDeclinedBills) {
                string userEmail = bill.Creditor.User.Email;
                ModelMap modelMap = new ModelMap();
                modelMap.Add("creditor", bill.Creditor.User);
                modelMap.Add("debitor", decliningDebitor.UserGroupMembership.User);
                modelMap.Add("userGroupDebitor", decliningDebitor);
                modelMap.Add("bill", bill);
                modelMap.Add("billUrl", billUrl);
                MailMessage mailMessage = EmailService.CreateMailMessage(userEmail, modelMap, "BillDeclined");
                EmailService.SendMessage(mailMessage);
            }
        }

        /// <summary>
        ///     Sendet eine Notification an alle Schuldner einer Rechnung mit dem Link zur Bestätigung
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="billUrl"></param>
        public void SendBillReceivedNotification(Bill bill, string billUrl) {
            Require.NotNull(bill, "bill");
            foreach (BillUserGroupDebitor billUserGroupDebitor in FindDebitorsToNotifyOnBillCreated(bill)) {
                string userEmail = billUserGroupDebitor.UserGroupMembership.User.Email;
                ModelMap modelMap = new ModelMap();
                modelMap.Add("debitor", billUserGroupDebitor.UserGroupMembership.User);
                modelMap.Add("creditor", bill.Creditor.User);
                modelMap.Add("billUrl", billUrl);
                modelMap.Add("bill", bill);
                modelMap.Add("amount", bill.GetPartialAmountByPortion(billUserGroupDebitor.Portion));
                MailMessage mailMessage = EmailService.CreateMailMessage(userEmail, modelMap, "BillReceived");
                EmailService.SendMessage(mailMessage);
            }

            foreach (var billGuestDebitor in bill.GuestDebitors) {
                string userEmail = billGuestDebitor.Email;
                ModelMap modelMap = new ModelMap();
                modelMap.Add("debitor", billGuestDebitor);
                modelMap.Add("creditor", bill.Creditor.User);
                modelMap.Add("billUrl", billUrl);
                modelMap.Add("bill", bill);
                modelMap.Add("amount", bill.GetPartialAmountByPortion(billGuestDebitor.Portion));
                MailMessage mailMessage = EmailService.CreateMailMessage(userEmail, modelMap, "BillReceivedGuest");
                EmailService.SendMessage(mailMessage);
            }

            
        }

        public void SendNotificationAboutUserActivation(User user, string editLink) {
            Require.NotNull(user, "user");
            ModelMap emailModel = new ModelMap();
            emailModel.Add("user", user);
            emailModel.Add("editLink", editLink);
            foreach (User admin in UserService.FindAllAdmins()) {
                MailMessage mailMessage = EmailService.CreateMailMessage(admin.Email, emailModel, "NotificationAboutUserActivation");
                EmailService.SendMessage(mailMessage);
            }
        }

        /// <summary>
        ///     Sendet eine Notification an denjenigen der eine Zahlung erhalten hat.
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="paymentUrl"></param>
        public void SendPaymentReceivedNotification(Payment payment, string paymentUrl) {
            Require.NotNull(payment, "payment");

            string userEmail = payment.Recipient.Membership.User.Email;
            ModelMap modelMap = new ModelMap();
            modelMap.Add("recipient", payment.Recipient.Membership.User);
            modelMap.Add("sender", payment.Sender.Membership.User);
            modelMap.Add("paymentUrl", paymentUrl);
            modelMap.Add("payment", payment);
            modelMap.Add("amount", payment.Amount);
            MailMessage mailMessage = EmailService.CreateMailMessage(userEmail, modelMap, "PaymentReceived");
            EmailService.SendMessage(mailMessage);
        }

        public void SendPeanutCommentNotification(Peanut peanut, string comment, PeanutUpdateNotificationOptions notificationOptions, User user) {
            Require.NotNull(peanut, "peanut");
            foreach (PeanutParticipation peanutParticipation in FindParticipatorsToNotifyOnUpdate(peanut, user)) {
                ModelMap modelMap = new ModelMap();
                modelMap.Add("peanut", peanut);
                modelMap.Add("user", user);
                modelMap.Add("recipient", peanutParticipation.UserGroupMembership.User);
                modelMap.Add("peanutUrl", notificationOptions.PeanutUrl);
                modelMap.Add("comment", comment);
                MailMessage mailMessage = EmailService.CreateMailMessage(peanutParticipation.UserGroupMembership.User.Email, modelMap, "PeanutComment");
                EmailService.SendMessage(mailMessage);
            }
        }

        public void SendPeanutInvitationNotification(Peanut peanut, User notifiedUser, PeanutInvitationNotificationOptions notificationOptions, User user) {
       
            Require.NotNull(notificationOptions, "notificationOptions");
            Require.NotNull(user, "user");

            if (notifiedUser.NotifyMeOnPeanutInvitation) {
                ModelMap modelMap = new ModelMap();
                modelMap.Add("peanut", peanut);
                modelMap.Add("user", user);
                modelMap.Add("recipient", notifiedUser);
                modelMap.Add("peanutUrl", notificationOptions.PeanutUrl);
                modelMap.Add("attendPeanutUrl", notificationOptions.AttendPeanutUrl);
                MailMessage mailMessage = EmailService.CreateMailMessage(notifiedUser.Email, modelMap, "PeanutInvitation");

                EmailService.SendMessage(mailMessage);
            }
        }

        public void SendPeanutUpdateNotification(Peanut peanut, PeanutDto dtoBeforeUpdate, IList<PeanutRequirement> requirementsBeforeUpdate,
            string updateComment, PeanutUpdateNotificationOptions notificationOptions, User user) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(notificationOptions, "notificationOptions");
            string[] differences = peanut.GetDto() - dtoBeforeUpdate;
            StringBuilder updateSummary = new StringBuilder();
            if (differences != null && differences.Any()) {
                foreach (string difference in differences) {
                    updateSummary.AppendLine(difference);
                }
            } else {
                updateSummary.AppendLine("-");
            }

            if (string.IsNullOrWhiteSpace(updateComment)) {
                updateComment = "-";
            }

            foreach (PeanutParticipation peanutParticipation in FindParticipatorsToNotifyOnUpdate(peanut, user)) {
                ModelMap modelMap = new ModelMap();
                modelMap.Add("peanut", peanut);
                modelMap.Add("peanutSourceName", dtoBeforeUpdate.Name);
                modelMap.Add("peanutSourceDay", dtoBeforeUpdate.Day);
                modelMap.Add("editor", user);
                modelMap.Add("recipient", peanutParticipation.UserGroupMembership.User);
                modelMap.Add("participation", peanutParticipation);
                modelMap.Add("peanutUrl", notificationOptions.PeanutUrl);
                modelMap.Add("updateSummary", updateSummary);
                modelMap.Add("comment", updateComment);
                MailMessage mailMessage = EmailService.CreateMailMessage(peanutParticipation.UserGroupMembership.User.Email, modelMap, "PeanutUpdated");
                EmailService.SendMessage(mailMessage);
            }
        }

        public void SendPeanutUpdateRequirementsNotification(Peanut peanut, string updateComment,
            PeanutUpdateRequirementsNotificationOptions notificationOptions, User user) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(notificationOptions, "notificationOptions");
            Require.NotNull(user, "user");
            if (string.IsNullOrWhiteSpace(updateComment)) {
                updateComment = "-";
            }

            foreach (PeanutParticipation creditors in FindCreditorsToNotifyOnRequirementsChange(peanut, user)) {
                ModelMap modelMap = new ModelMap();
                modelMap.Add("peanut", peanut);
                modelMap.Add("editor", user);
                modelMap.Add("recipient", creditors.UserGroupMembership.User);
                modelMap.Add("peanutUrl", notificationOptions.PeanutUrl);
                modelMap.Add("requirements",
                    string.Join(Environment.NewLine, peanut.Requirements.Select(r => string.Format("<li>{0} {1}</li>", r.QuantityAndUnit, r.Name))));
                modelMap.Add("updateComment", updateComment);

                MailMessage mailMessage = EmailService.CreateMailMessage(creditors.UserGroupMembership.User.Email,
                    modelMap,
                    "PeanutRequirementsUpdated");
                EmailService.SendMessage(mailMessage);
            }
        }

        public void SendPeanutUpdateStateNotification(Peanut peanut, PeanutUpdateNotificationOptions notificationOptions, User user) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(notificationOptions, "notificationOptions");

            if (peanut.PeanutState == PeanutState.Started) {
                /*Der Peanut wurde gestartet => alle Teilnehmer, außer den ändernden Nutzer benachrichtigen*/
                foreach (PeanutParticipation peanutParticipation in FindParticipatorsToNotifyOnUpdate(peanut, user)) {
                    ModelMap modelMap = new ModelMap();
                    modelMap.Add("peanut", peanut);
                    modelMap.Add("editor", user);
                    modelMap.Add("recipient", peanutParticipation.UserGroupMembership.User);
                    modelMap.Add("participation", peanutParticipation);
                    modelMap.Add("peanutUrl", notificationOptions.PeanutUrl);
                    MailMessage mailMessage = EmailService.CreateMailMessage(peanutParticipation.UserGroupMembership.User.Email,
                        modelMap,
                        "PeanutStart");
                    EmailService.SendMessage(mailMessage);
                }
            }

            if (peanut.PeanutState == PeanutState.PurchasingDone) {
                /*Der Beschaffung der Voraussetzungen für den Peanut ist abgeschlossen => alle Produzenten, außer den ändernden Nutzer, benachrichtigen*/
                foreach (PeanutParticipation peanutParticipation in FindProducersToNotifyOnPurchasingDone(peanut, user)) {
                    ModelMap modelMap = new ModelMap();
                    modelMap.Add("peanut", peanut);
                    modelMap.Add("editor", user);
                    modelMap.Add("recipient", peanutParticipation.UserGroupMembership.User);
                    modelMap.Add("participation", peanutParticipation);
                    modelMap.Add("peanutUrl", notificationOptions.PeanutUrl);
                    MailMessage mailMessage = EmailService.CreateMailMessage(peanutParticipation.UserGroupMembership.User.Email,
                        modelMap,
                        "PeanutPurchasingDone");
                    EmailService.SendMessage(mailMessage);
                }
            }

            if (peanut.PeanutState == PeanutState.Canceled) {
                /*Der Peanut wurde abgesagt => alle Teilnehmer, außer den ändernden Nutzer benachrichtigen*/
                foreach (PeanutParticipation peanutParticipation in FindParticipatorsToNotifyOnUpdate(peanut, user)) {
                    ModelMap modelMap = new ModelMap();
                    modelMap.Add("peanut", peanut);
                    modelMap.Add("editor", user);
                    modelMap.Add("recipient", peanutParticipation.UserGroupMembership.User);
                    modelMap.Add("participation", peanutParticipation);
                    modelMap.Add("peanutUrl", notificationOptions.PeanutUrl);
                    MailMessage mailMessage = EmailService.CreateMailMessage(peanutParticipation.UserGroupMembership.User.Email,
                        modelMap,
                        "PeanutCanceled");
                    EmailService.SendMessage(mailMessage);
                }
            }
        }

        /// <summary>
        ///     Sendet eine Notification an den Nutzer mit der Information das sich ein Nutzer um eine Mitgliedschaft in einer
        ///     Gruppe beworben hat.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="urlToUserGroup"></param>
        public void SendRequestMembershipNotification(User user, string urlToUserGroup) {
            Require.NotNull(user, "user");
            ModelMap modelMap = new ModelMap();
            modelMap.Add("user", user);
            modelMap.Add("groupLink", urlToUserGroup);
            MailMessage mailMessage = EmailService.CreateMailMessage(user.Email, modelMap, "RequestMembership");
            EmailService.SendMessage(mailMessage);
        }

        /// <summary>
        ///     Sendet eine Notification an den Nutzer mit der Information das sich ein Nutzer um eine Mitgliedschaft in einer
        ///     Gruppe beworben hat.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userGroup"></param>
        /// <param name="urlToUserGroup"></param>
        public void SendConfirmMembershipNotification(User user,UserGroup userGroup, string urlToUserGroup) {
            Require.NotNull(user, "user");
            ModelMap modelMap = new ModelMap();
            modelMap.Add("user", user);
            modelMap.Add("groupName", userGroup.DisplayName);
            modelMap.Add("groupLink", urlToUserGroup);
            MailMessage mailMessage = EmailService.CreateMailMessage(user.Email, modelMap, "ConfirmMembership");
            EmailService.SendMessage(mailMessage);
        }

        /// <summary>
        ///     Sendet eine Notification an den Nutzer das er zu einer Mitgliedschaft in einer Gruppe eingeladen wurde
        /// </summary>
        /// <param name="user"></param>
        /// <param name="allMembershipsUrl"></param>
        public void SendUserGroupInvitationNotification(User user, string allMembershipsUrl) {
            Require.NotNull(user, "user");
            ModelMap modelMap = new ModelMap();
            modelMap.Add("user", user);
            modelMap.Add("groupLink", allMembershipsUrl);
            MailMessage mailMessage = EmailService.CreateMailMessage(user.Email, modelMap, "InviteMembership");
            EmailService.SendMessage(mailMessage);
        }

        private IList<PeanutParticipation> FindCreditorsToNotifyOnRequirementsChange(Peanut peanut, User user) {
            return
                    peanut.ConfirmedParticipations.Where(
                                part =>
                                    part.ParticipationType.IsCreditor && part.UserGroupMembership.User.NotifyMeAsCreditorOnPeanutRequirementsChanged
                                    && !part.UserGroupMembership.User.Equals(user))
                            .ToList();
        }

        private static IEnumerable<BillUserGroupDebitor> FindDebitorsToNotifyOnBillCreated(Bill bill) {
            /*Alle Schuldner*/
            IList<BillUserGroupDebitor> allDebitors = bill.UserGroupDebitors;
            /*außer der Gläubiger der Rechnung, falls dieser ebenfalls beteiligt sein sollte.*/
            IList<BillUserGroupDebitor> findDebitorsToNotifyOnBillCreated =
                    allDebitors.Where(ugd => !ugd.UserGroupMembership.Equals(bill.Creditor) &&
                                             /*Nur die, die benachrichtigt werden wollen.*/
                                             ugd.UserGroupMembership.User.NotifyMeAsDebitorOnIncomingBills)
                            .ToList();
            return findDebitorsToNotifyOnBillCreated;
        }

        /// <summary>
        ///     Sucht nach Teilnehmern an einem Peanut, die eine Benachrichtigung erhalten müssen, nachdem ein Peanut geändert
        ///     wurde.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        private IList<PeanutParticipation> FindParticipatorsToNotifyOnUpdate(Peanut peanut, User editor) {
            List<PeanutParticipation> participations = peanut.Participations.Where(part =>
                    /*Nur bestätigte Teilnahmen*/
                        part.ParticipationState == PeanutParticipationState.Confirmed &&
                        /*Der Bearbeiter braucht keine Benachrichtigung erhalten*/
                        !part.UserGroupMembership.User.Equals(editor) &&
                        /*Nur die Nutzer, die benachrichtigt werden wollen*/
                        part.UserGroupMembership.User.NotifyMeAsParticipatorOnPeanutChanged
            ).ToList();

            return participations;
        }

        private IList<PeanutParticipation> FindProducersToNotifyOnPurchasingDone(Peanut peanut, User user) {
            return
                    peanut.ConfirmedParticipations.Where(
                                part =>
                                    part.ParticipationType.IsProducer
                                    && part.UserGroupMembership.User.NotifyMeAsParticipatorOnPeanutChanged
                                    && !part.UserGroupMembership.User.Equals(user))
                            .ToList();
        }
    }
}