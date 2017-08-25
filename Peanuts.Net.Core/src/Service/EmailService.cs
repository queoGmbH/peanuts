using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement;

using Common.Logging;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public class EmailService : IEmailService {
        private const string MAIL_MESSAGE_SUBJECT_MARKER = "Subject: ";
        private readonly ILog _logger = LogManager.GetLogger(typeof(EmailService));

        private string _emailSenderName;

        /// <summary>
        ///     Liefert oder setzt den Provider welcher E-Mailtexte liefert.
        /// </summary>
        public IMessageProvider EmailMessageProvider { get; set; }

        /// <summary>
        ///     Liefert oder setzt die Absenderadresse die in der Email verwendet wird.
        /// </summary>
        public string EmailSenderAddress { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Klartextnamen des Absenders der in der Email verwendet wird.
        /// </summary>
        public string EmailSenderName {
            get { return _emailSenderName; }
            set { _emailSenderName = value; }
        }

        /// <summary>
        ///     Liefert oder setzt einen Wert der angibt, ob ein invalides SSL Zertifikat beim Senden
        ///     der Emails ignoriert werden soll.
        /// </summary>
        public bool IgnoreInvalidSslCertificate { get; set; }

        /// <summary>
        ///     Liefert oder setzt die Hostadresse des SMTP-Servers der für den Versand verwendet wird.
        /// </summary>
        public string SmtpHostAddress { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Port des SMTP-Servers der beim Versand verwendet wird.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Login mit dem sich die Anwendung am SMTP-Server anmeldet.
        /// </summary>
        public string SmtpServerLogin { get; set; }

        /// <summary>
        ///     Liefert oder setzt das Passwort.
        /// </summary>
        public string SmtpServerPassword { get; set; }

        /// <summary>
        ///     Liefert oder setzt einen Wert, der angibt, ob beim Versand einer EMail SSL verwendet wird.
        /// </summary>
        public bool SmtpSslEnabled { get; set; }

        /// <summary>
        ///     Erzeugt eine neue <see cref="MailMessage" />
        /// </summary>
        /// <param name="to">Der oder die Empfänger. Mehrere Empfänger müssen durch ein Komma "," getrennt sein.</param>
        /// <param name="model">Die Daten mit denen das Template befüllt werden soll.</param>
        /// <param name="mailTemplateName">Name des Templates.</param>
        /// <returns>Die erzeugte MailMessage</returns>
        public MailMessage CreateMailMessage(string to, ModelMap model, string mailTemplateName) {
            string mailTemplate = EmailMessageProvider.RenderMessage(mailTemplateName, model);
            string subject = GetMailMessageSubject(mailTemplate);
            string body = GetMailMessageBody(mailTemplate);
            _logger.Info($"Create MailMessage with {subject} to {to}.");
            MailMessage mailMessage = new MailMessage(new MailAddress(EmailSenderAddress, EmailSenderName), new MailAddress(to, to));
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
              _logger.Info($"MailMessage with {mailMessage.From} to {mailMessage.To.First().Address}.");
            return mailMessage;
        }

    

        /// <summary>
        ///     Versendet eine MailMessage.
        /// </summary>
        /// <param name="mailMessage"></param>
        public void SendMessage(MailMessage mailMessage) {
             _logger.Info($"Send Message to {mailMessage.To.First().Address}");
            using (SmtpClient smtpClient = GetConfiguredSmtpClient()) {
                try {
                    _logger.DebugFormat("Address:Port {0}:{1}", smtpClient.Host, smtpClient.Port);
                    smtpClient.Send(mailMessage);
                    _logger.InfoFormat("Mail wurde versendet an:", mailMessage.To.First().Address);
                } catch (Exception exception) {
                    _logger.ErrorFormat("Mail wurde nicht versendet. Empfänger: {0}", exception, mailMessage.To.First().Address);
                    throw;
                }
            }
        }

        private SmtpClient GetConfiguredSmtpClient() {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = SmtpHostAddress;
            smtpClient.Port = SmtpPort;
            smtpClient.EnableSsl = SmtpSslEnabled;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            if (!(string.IsNullOrWhiteSpace(SmtpServerLogin) || string.IsNullOrWhiteSpace(SmtpServerPassword))) {
                smtpClient.Credentials = new NetworkCredential(SmtpServerLogin, SmtpServerPassword);
            }
            if (IgnoreInvalidSslCertificate) {
                ServicePointManager.ServerCertificateValidationCallback = delegate {
                    return true;
                };
            }
            return smtpClient;
        }

   

        private string GetMailMessageBody(string mailTemplate) {
            string[] lines = mailTemplate.Split('\r', '\n');
            string firstLine = lines.FirstOrDefault(x => x.Contains(MAIL_MESSAGE_SUBJECT_MARKER));
            int indexOf = firstLine != null ? firstLine.Length : -1;
            return mailTemplate.Remove(0, indexOf).TrimStart('\r', '\n', ' ');
        }

        private string GetMailMessageSubject(string mailTemplate) {
            string[] lines = mailTemplate.Split('\r', '\n');
            string firstLine = lines.FirstOrDefault(x => x.Contains(MAIL_MESSAGE_SUBJECT_MARKER));
            if (firstLine == null) {
                return "";
            }
            return firstLine.Remove(0, MAIL_MESSAGE_SUBJECT_MARKER.Length).TrimStart('\r', '\n', ' ');
        }
    }
}