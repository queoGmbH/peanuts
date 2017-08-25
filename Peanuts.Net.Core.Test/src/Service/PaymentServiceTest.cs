using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders;

using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    [TestFixture]
    public class PaymentServiceTest : ServiceBaseTest {

        /// <summary>
        ///    Liefert oder setzt den PaymentService
        /// </summary>
        public IPaymentService PaymentService { get; set; }

        /// <summary>
        ///    Liefert oder setzt den UserDao
        /// </summary>
        public IUserDao UserDao { get; set; }

        public PaymentServiceTest() {
            
        }



        /// <summary>
        /// Testet das Erstellen einer Zahlung
        /// </summary>
        [Test]
        public void TestCreatePayment() {

            //Given: Zwei Konten
            User requestRecipient = Create.A.User();
            User requestSender =Create.A.User();
            UserGroup userGroup = Create.A.UserGroup();
            UserGroupMembership userGroupMemberShipSender = Create.A.UserGroupMembership().ForUser(requestSender).InUserGroup(userGroup);
            UserGroupMembership userGroupMemberShipRecipient = Create.A.UserGroupMembership().ForUser(requestRecipient).InUserGroup(userGroup);
            Account accountSender =userGroupMemberShipSender.Account;
            Account accountRecipient = userGroupMemberShipRecipient.Account;

            //When: Eine Buchung von einem Konto auf das andere Konto erfolgen soll
            const double AMOUNT = 20.45;

            const string TEST_PAYMENT = "Test Payment";
            PaymentDto paymentDto = new PaymentDto() {Amount = AMOUNT,PaymentType = PaymentType.Cash, Text = TEST_PAYMENT};
            User creator = Create.A.User();
            Payment payment = PaymentService.Create(paymentDto,accountRecipient, accountSender, requestRecipient, requestSender, creator, String.Empty);

            //Then: Muss das korrekt funktionieren, und zwei Buchungseinträge mit derselben Nummer erstellt werden.
            payment.Id.Should().BeGreaterThan(0);
            payment.Amount.Should().Be(AMOUNT);
            payment.Text.Should().Be(TEST_PAYMENT);
            payment.PaymentType.Should().Be(PaymentType.Cash);
            payment.RequestRecipient.Should().Be(requestRecipient);
            payment.RequestSender.Should().Be(requestSender);
            payment.CreatedBy.Should().Be(creator);
            payment.Recipient.Should().Be(accountRecipient);
            payment.Sender.Should().Be(accountSender);

        }
        
        
        /// <summary>
        /// Testet das Erstellen einer Zahlung
        /// </summary>
        [Test]
        public void TestAcceptPayment() {

            //Given: Zwei Konten
            UserGroup userGroup = Create.A.UserGroup().Build();
            User recipient = Create.A.User().Build();
            User sender = Create.A.User().Build();
            UserGroupMembership recipientMembership = Create.A.UserGroupMembership().ForUser(recipient).InUserGroup(userGroup).Build();
            UserGroupMembership senderMembership = Create.A.UserGroupMembership().ForUser(sender).InUserGroup(userGroup).Build();

            const double AMOUNT = 10.20;
            Payment payment = Create.A.Payment().FromSender(senderMembership).ToRecipient(recipientMembership).WithAmount(AMOUNT);



            //When: Eine Buchung von einem Konto auf das andere Konto erfolgen soll
        
            PaymentService.AcceptPayment(payment, recipient);

            //Then: Muss das korrekt funktionieren, und zwei Buchungseinträge mit derselben Nummer erstellt werden.
            payment.PaymentStatus.Should().Be(PaymentStatus.Accecpted);
            recipientMembership.Account.Balance.Should().Be(-AMOUNT);
            senderMembership.Account.Balance.Should().Be(AMOUNT);

        }

    }
}