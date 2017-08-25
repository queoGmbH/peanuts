using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders;

using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {

    [TestFixture]
    public class BookingServiceTest : ServiceBaseTest {

        public IBookingService BookingService { get; set; }

        public BookingServiceTest() {
            
        }



        /// <summary>
        /// Testet das Simple Buchen
        /// </summary>
        [Test]
        public void TestBookSimple() {

            //Given: Zwei Konten
            Account accountSender = Create.An.Account();
            Account accountRecipient = Create.An.Account();

            //When: Eine Buchung von einem Konto auf das andere Konto erfolgen soll
            const double AMOUNT = 20.45;
            string simpleBuchung = "Simple Buchung";
            Booking booking = BookingService.Book(accountSender, accountRecipient, AMOUNT, simpleBuchung);

            //Then: Muss das korrekt funktionieren, und zwei Buchungseinträge mit derselben Nummer erstellt werden.
            booking.Id.Should().BeGreaterThan(0);
            booking.Amount.Should().Be(AMOUNT);
            booking.SenderAccountEntry.Should().NotBeNull();
            booking.RecipientAccountEntry.Should().NotBeNull();

            booking.SenderAccountEntry.Amount.Should().Be(-AMOUNT);
            booking.RecipientAccountEntry.Amount.Should().Be(AMOUNT);

            accountSender.Balance.Should().Be(-AMOUNT);
            accountRecipient.Balance.Should().Be(AMOUNT);
        }

    }
}