using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace Com.QueoFlow.Peanuts.Net.Web {
    public class SmsService : IIdentityMessageService {
        public Task SendAsync(IdentityMessage message) {
            // Hier den SMS-Dienst einfügen, um eine Textnachricht zu senden.
            return Task.FromResult(0);
        }
    }
}