using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Web.Models.Payment;
using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [RoutePrefix("Payment")]
    [Authorization]
    public class PaymentController : Controller {
        public IPaymentService PaymentService { get; set; }

        /// <summary>
        ///     Ruft die Url ab, an die weitergeleitet wird, wenn die Zahlung per PayPal erfolgen soll.
        /// </summary>
        public string PayPalUrl { get; set; }

        public IUserGroupService UserGroupService { get; set; }

        [Route("Accepted")]
        public ActionResult AcceptedPayments(User currentUser, int pageNumber = 1, int pageSize = 25) {
            Require.NotNull(currentUser, "currentUser");

            IPage<Payment> pendingPayments = PaymentService.FindPendingPaymentsByUser(PageRequest.All, currentUser);
            IList<Payment> pendingPaymentsAsSender = pendingPayments.Where(payment => payment.RequestSender.Equals(currentUser)).ToList();
            IList<Payment> pendingPaymentsAsRecipient = pendingPayments.Where(payment => payment.RequestRecipient.Equals(currentUser)).ToList();

            IPage<Payment> acceptedPayments = PaymentService.FindAcceptedPaymentsByUser(new PageRequest(pageNumber, pageSize), currentUser);
            IPage<Payment> declinedPayments = PaymentService.FindDeclinedPaymentsByUser(PageRequest.First, currentUser);

            return View("AcceptedPayments",
                new PendingPaymentsViewModel(currentUser, pendingPaymentsAsSender, pendingPaymentsAsRecipient, declinedPayments, acceptedPayments));
        }

        [Route("{payment}/Status")]
        [HttpPatch]
        public ActionResult AcceptPayment(Payment payment, User currentUser) {
            Require.NotNull(payment, "payment");
            Require.NotNull(currentUser, "currentUser");

            if (!ModelState.IsValid) {
                return View("AcceptPayment", payment);
            }

            PaymentService.AcceptPayment(payment, currentUser);
            return RedirectToAction("Index");
        }

        [Route("{payment}/AcceptForm")]
        public ActionResult AcceptPaymentForm(Payment payment, User currentUser) {
            Require.NotNull(payment, "payment");

            return View("AcceptPayment", payment);
        }

        [Route("Declined")]
        public ActionResult DeclinedPayments(User currentUser) {
            Require.NotNull(currentUser, "currentUser");

            IPage<Payment> pendingPayments = PaymentService.FindPendingPaymentsByUser(PageRequest.All, currentUser);
            IList<Payment> pendingPaymentsAsSender = pendingPayments.Where(payment => payment.RequestSender.Equals(currentUser)).ToList();
            IList<Payment> pendingPaymentsAsRecipient = pendingPayments.Where(payment => payment.RequestRecipient.Equals(currentUser)).ToList();

            IPage<Payment> acceptedPayments = PaymentService.FindAcceptedPaymentsByUser(PageRequest.First, currentUser);
            IPage<Payment> declinedPayments = PaymentService.FindDeclinedPaymentsByUser(PageRequest.All, currentUser);

            return View("DeclinedPayments",
                new PendingPaymentsViewModel(currentUser, pendingPaymentsAsSender, pendingPaymentsAsRecipient, declinedPayments, acceptedPayments));
        }

        [Route("{payment}/Status")]
        [HttpDelete]
        public ActionResult DeclinePayment(Payment payment, DeclinePaymentCommand declinePaymentCommand, User currentUser) {
            Require.NotNull(payment, "payment");
            Require.NotNull(declinePaymentCommand, "declinePaymentCommand");

            if (!ModelState.IsValid) {
                return View("DeclinePayment", new DeclinePaymentViewModel(payment, declinePaymentCommand));
            }

            PaymentService.DeclinePayment(payment, declinePaymentCommand.DeclineReason, currentUser);
            return RedirectToAction("Index");
        }

        [Route("{payment}/DeclineForm")]
        public ActionResult DeclinePaymentForm(Payment payment, User currentUser) {
            Require.NotNull(payment, "payment");

            return View("DeclinePayment", new DeclinePaymentViewModel(payment));
        }

        [HttpDelete]
        [Route("{payment}")]
        public ActionResult Delete(Payment payment) {
            Require.NotNull(payment, "payment");

            if (!ModelState.IsValid) {
                return View("Delete", payment);
            }

            PaymentService.Delete(payment);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("{payment}/DeleteForm")]
        public ActionResult DeleteForm(Payment payment) {
            Require.NotNull(payment, "payment");

            return View("Delete", payment);
        }

        [Route("In")]
        [HttpPost]
        public ActionResult GotMoney(GotMoneyCommand gotMoneyCommand, User currentUser) {
            if (!ModelState.IsValid) {
                IList<UserGroupMembership> otherMembershipsInUsersGroups = FindOtherMembershipsInUsersGroups(currentUser, null);
                return View("GotMoney", new GotMoneyViewModel(otherMembershipsInUsersGroups, gotMoneyCommand));
            }

            UserGroupMembership sender = UserGroupService.FindMembershipsByUserAndGroup(currentUser, gotMoneyCommand.Sender.UserGroup);

            string paymentUrl = Url.Action("PendingPayments", "Payment", null, Request.Url.Scheme);
            PaymentService.Create(gotMoneyCommand.PaymentDto,
                sender.Account,
                gotMoneyCommand.Sender.Account,
                gotMoneyCommand.Sender.User,
                sender.User,
                currentUser,
                paymentUrl);

            return RedirectToAction("Index", "Payment");
        }

        [Route("In/CreateForm")]
        [HttpGet]
        public ActionResult GotMoneyForm(UserGroup userGroup, User currentUser) {
            IList<UserGroupMembership> otherMembershipsInUsersGroups = FindOtherMembershipsInUsersGroups(currentUser, userGroup);

            return View("GotMoney", new GotMoneyViewModel(otherMembershipsInUsersGroups));
        }

        [Route("")]
        public ActionResult Index(User currentUser, PaginationCommand acceptedPaymentsPaginationCommand,
            PaginationCommand declinedPaymentsPaginationCommand) {
            Require.NotNull(currentUser, "currentUser");

            IPage<Payment> pendingPayments = PaymentService.FindPendingPaymentsByUser(new PageRequest(1, 1), currentUser);
            IPage<Payment> declinedPayments = PaymentService.FindDeclinedPaymentsByUser(new PageRequest(1, 1), currentUser);

            if (declinedPayments.TotalElements > 0) {
                /*Es gibt abgelehnte Zahlungen => gehe dorthin*/
                return RedirectToAction("DeclinedPayments");
            } else if (pendingPayments.TotalElements > 0) {
                /*Es gibt abgelehnte Zahlungen => gehe dorthin*/
                return RedirectToAction("PendingPayments");
            } else {
                /*Zeige die vergangenen Zahlungen an*/
                return RedirectToAction("AcceptedPayments");
            }
        }

        [HttpPost]
        [Route("Out")]
        public ActionResult PayMoney(PayMoneyCommand payMoneyCommand, User currentUser) {
            Require.NotNull(payMoneyCommand, "payMoneyCommand");

            if (!ModelState.IsValid) {
                IList<UserGroupMembership> otherMembershipsInUsersGroups = FindOtherMembershipsInUsersGroups(currentUser, null);
                return View("PayMoney", new PayMoneyViewModel(otherMembershipsInUsersGroups, payMoneyCommand));
            }

            UserGroupMembership sender = UserGroupService.FindMembershipsByUserAndGroup(currentUser, payMoneyCommand.Recipient.UserGroup);

            switch (payMoneyCommand.PaymentDto.PaymentType) {
                case PaymentType.PayPal: {
                    return PayWithPayPal(payMoneyCommand, sender, currentUser);
                }
                case PaymentType.Cash: {
                    return PayCash(payMoneyCommand, sender, currentUser);
                }
                default: {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        [Route("Out/CreateForm")]
        public ActionResult PayMoneyForm(UserGroup userGroup, User currentUser) {
            IList<UserGroupMembership> otherMembershipsInUsersGroups = FindOtherMembershipsInUsersGroups(currentUser, userGroup);
            return View("PayMoney", new PayMoneyViewModel(otherMembershipsInUsersGroups));
        }

        [Route("{payment}/PayPalSuccess")]
        public ActionResult PayPalCancel(Payment payment) {
            Require.NotNull(payment, "payment");
            Require.IsTrue(() => payment.PaymentType == PaymentType.PayPal, "payment");

            PaymentService.Delete(payment);

            return RedirectToAction("AllMemberships", "UserGroup");
        }

        public ActionResult PayPalSuccess(Payment payment, User currentUser) {
            Require.NotNull(payment, "payment");
            Require.IsTrue(() => payment.PaymentType == PaymentType.PayPal, "payment");

            PaymentService.AcceptPayment(payment, currentUser);

            return RedirectToAction("AllMemberships", "UserGroup");
        }

        [Route("Pending")]
        public ActionResult PendingPayments(User currentUser) {
            Require.NotNull(currentUser, "currentUser");

            IPage<Payment> pendingPayments = PaymentService.FindPendingPaymentsByUser(PageRequest.All, currentUser);
            IList<Payment> pendingPaymentsAsSender = pendingPayments.Where(payment => payment.RequestSender.Equals(currentUser)).ToList();
            IList<Payment> pendingPaymentsAsRecipient = pendingPayments.Where(payment => payment.RequestRecipient.Equals(currentUser)).ToList();

            IPage<Payment> acceptedPayments = PaymentService.FindAcceptedPaymentsByUser(PageRequest.First, currentUser);
            IPage<Payment> declinedPayments = PaymentService.FindDeclinedPaymentsByUser(PageRequest.First, currentUser);

            return View("PendingPayments",
                new PendingPaymentsViewModel(currentUser, pendingPaymentsAsSender, pendingPaymentsAsRecipient, declinedPayments, acceptedPayments));
        }

        private IList<UserGroupMembership> FindOtherMembershipsInUsersGroups(User user, UserGroup onlyInGroup = null) {
            IList<UserGroupMembership> otherMembershipsInUsersGroups =
                    UserGroupService
                            .FindOtherMembershipsInUsersGroups(PageRequest.All,
                                user,
                                new[] { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member })
                            .Where(memberShip => !memberShip.User.Equals(user))
                            .ToList();
            if (onlyInGroup != null) {
                otherMembershipsInUsersGroups = otherMembershipsInUsersGroups.Where(mem => mem.UserGroup.Equals(onlyInGroup)).ToList();
            }
            return otherMembershipsInUsersGroups;
        }

        private ActionResult PayCash(PayMoneyCommand payMoneyCommand, UserGroupMembership sender, User currentUser) {
            string paymentUrl = Url.Action("PendingPayments", "Payment", null, Request.Url.Scheme);
            PaymentService.Create(payMoneyCommand.PaymentDto,
                payMoneyCommand.Recipient.Account,
                sender.Account,
                payMoneyCommand.Recipient.User,
                sender.User,
                currentUser,
                paymentUrl);

            return RedirectToAction("Index", "Payment");
        }

        private ActionResult PayWithPayPal(PayMoneyCommand payMoneyCommand, UserGroupMembership sender, User currentUser) {
            string paymentUrl = Url.Action("PendingPayments", "Payment", null, Request.Url.Scheme);
            Payment payment = PaymentService.Create(payMoneyCommand.PaymentDto,
                payMoneyCommand.Recipient.Account,
                sender.Account,
                payMoneyCommand.Recipient.User,
                sender.User,
                currentUser,
                paymentUrl);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PayPalUrl);
            webRequest.Method = "post";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServerCertificateValidationCallback = delegate {
                return true;
            };
            ServicePointManager.ServerCertificateValidationCallback = delegate {
                return true;
            };
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            PayPalPaymentCommand payPalPaymentCommand = new PayPalPaymentCommand(
                payMoneyCommand.PaymentDto.Amount,
                payMoneyCommand.PaymentDto.Text,
                payMoneyCommand.Recipient.User,
                Url.Action("PayPalSuccess", "Payment", new { payment = payment.BusinessId }, Request.Url.Scheme),
                Url.Action("PayPalCancel", "Payment", new { payment = payment.BusinessId }, Request.Url.Scheme)
            );
            string postData = payPalPaymentCommand.ToString();
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(postData);

            using (Stream dataStream = webRequest.GetRequestStream()) {
                dataStream.Write(postBytes, 0, postBytes.Length);
            }

            WebResponse response = webRequest.GetResponse();

            /*Weiterleiten*/
            return Redirect(response.ResponseUri.ToString());
        }
    }
}