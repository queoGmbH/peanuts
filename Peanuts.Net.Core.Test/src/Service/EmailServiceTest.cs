using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement;

using nDumbster.Smtp;

using NHibernate.Util;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    [TestFixture]
    public class EmailServiceTest  {
        private SimpleSmtpServer _smtpServer;

        [SetUp]
        public void Setup() {
            _smtpServer = SimpleSmtpServer.Start();
        }

        [TearDown]
        public void Cleanup() {
            _smtpServer.Stop();
        }


        [Test]
        public void TestCreateMessage() {
            EmailService emailService = GetEmailService();

            MailMessage mailMessage = emailService.CreateMailMessage("test@test.com", new ModelMap(), "default");
            Assert.AreEqual("test@test.com", mailMessage.To.First().Address);
            Assert.AreEqual("Der Mailbetreff", mailMessage.Subject);
            Assert.AreEqual(@"Hallo Nutzer,

das ist eine Mail für dich.

Viele Grüße
Tester", mailMessage.Body);
        }

        [Test]
        public void TestSendMailMessage() {
            EmailService emailService = GetEmailService();
            MailMessage mailMessage = emailService.CreateMailMessage("test@test.com", new ModelMap(), "default");
            emailService.SendMessage(mailMessage);

            MailMessage receivedEmail = (MailMessage)_smtpServer.ReceivedEmail.FirstOrNull();
            Assert.AreEqual(1, _smtpServer.ReceivedEmailCount);
            Assert.AreEqual(mailMessage.Body, receivedEmail.Body);
            Assert.AreEqual(mailMessage.Subject, receivedEmail.Subject);
            Assert.AreEqual(mailMessage.To.ToString(), receivedEmail.To.ToString());
        }

        private EmailService GetEmailService() {
            EmailService emailService = new EmailService();
            emailService.EmailMessageProvider = new StaticMailMessageProvider();
            emailService.EmailSenderAddress = "test@test.com";
            emailService.EmailSenderName = "tester";
            emailService.SmtpHostAddress = "localhost";
            emailService.SmtpPort = 25;

            return emailService;
        }
    }

    /// <summary>
    ///     Klasse für die Bereitstellungen von Testtexten in Tests.
    /// </summary>
    internal class StaticMailMessageProvider : IMessageProvider {
        private readonly Dictionary<string, string> _mailMessages = new Dictionary<string, string>() {
            {
                "default", @"Subject: Der Mailbetreff
Hallo Nutzer,

das ist eine Mail für dich.

Viele Grüße
Tester"
            }
        };

        /// <summary>
        ///     Rendert eine MailMessage aus dem angegebenen Template und verwendet dabei die Daten aus dem Model.
        /// </summary>
        /// <param name="templateName">Name des Templates</param>
        /// <param name="model">Daten für das Template</param>
        /// <returns></returns>
        public string RenderMessage(string templateName, ModelMap model)
        {
            if (_mailMessages.ContainsKey(templateName)) {
                return _mailMessages[templateName];
            }
            return _mailMessages["default"];
        }
    }
}