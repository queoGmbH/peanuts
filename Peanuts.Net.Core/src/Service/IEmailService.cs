using System.Net.Mail;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public interface IEmailService {
        /// <summary>
        ///     Erzeugt eine neue Email.
        /// </summary>
        /// <param name="to">Empfänger</param>
        /// <param name="model">Daten</param>
        /// <param name="mailTemplateName">Name des Templates für die Email</param>
        /// <returns>Die <see cref="MailMessage">Email</see></returns>
        MailMessage CreateMailMessage(string to, ModelMap model, string mailTemplateName);

        /// <summary>
        ///     Versendet die Mail.
        /// </summary>
        /// <param name="mailMessage"></param>
        void SendMessage(MailMessage mailMessage);

  
    }
}