using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Implementierung für <see cref="IAuthenticationSessionStore" /> welches die <see cref="AuthenticationTicket" />s
    ///     global in einem Dictionary speichert.
    /// </summary>
    /// <remarks>
    ///     Wenn viele Daten im AuthenticationTicket gespeichert werden, sollte diese Implementierung verwendet werden, damit
    ///     der Cookie nicht
    ///     so groß wird.
    ///     Zur Verwendung muss eine Instanz davon an den <see cref="T:CookieAuthenticationOptions.SessionStore" /> übergeben
    ///     werden.
    ///     <code>
    /// app.UseCookieAuthentication(new CookieAuthenticationOptions
    ///        {
    ///         AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
    ///         LoginPath = new PathString("/Account/Login"),
    ///         SessionStore = new GlobalDictionarySessionStore()
    ///     }); 
    /// </code>
    /// </remarks>
    public class GlobalDictionarySessionStore : IAuthenticationSessionStore {
        private readonly ILog _logger = LogManager.GetLogger<GlobalDictionarySessionStore>();
        readonly ConcurrentDictionary<string, AuthenticationTicket> _ticketStore = new ConcurrentDictionary<string, AuthenticationTicket>();

        public Task RemoveAsync(string key) {
            AuthenticationTicket ticket;
            bool isRemoved = _ticketStore.TryRemove(key, out ticket);
            if (_logger.IsDebugEnabled) {
                string identityName = (ticket == null || ticket.Identity == null) ? "null" : ticket.Identity.Name;
                _logger.DebugFormat("Ticket with key {0} for {1} was tried to remove with result {2}", key, identityName, isRemoved);
            }
            return Task.FromResult<object>(null);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket) {
            AuthenticationTicket authenticationTicket = _ticketStore[key];
            bool isTicketUpdated = _ticketStore.TryUpdate(key, ticket, authenticationTicket);
            if (_logger.IsDebugEnabled) {
                string identityName = (ticket == null || ticket.Identity == null) ? "null" : ticket.Identity.Name;
                _logger.DebugFormat("Ticket with key {0} for {1} was tried to update with result {2}",
                    key,
                    identityName,
                    isTicketUpdated);
            }
            Cleanup();
            return Task.FromResult<object>(null);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key) {
            AuthenticationTicket authenticationTicket;
            try {
                authenticationTicket = _ticketStore[key];
            } catch (KeyNotFoundException) {
                if (_logger.IsInfoEnabled) {
                    _logger.InfoFormat("Tryed to retrieve ticket for {0} but was not found.", key);
                }
                authenticationTicket = null;
            }
            return Task.FromResult(authenticationTicket);
        }

        public Task<string> StoreAsync(AuthenticationTicket ticket) {
            string key = Guid.NewGuid().ToString();
            bool isAdded = _ticketStore.TryAdd(key, ticket);
            if (_logger.IsDebugEnabled) {
                string identityName = (ticket == null ||ticket.Identity == null) ? "null" : ticket.Identity.Name;
                _logger.DebugFormat("Tryed to store ticket with key {0} for identity {1} with result {2}", key, identityName, isAdded);
            }
            Cleanup();
            return Task.FromResult(key);
        }

        private void Cleanup() {
            int initialTicketsInStore = _ticketStore.Count;
            foreach (KeyValuePair<string, AuthenticationTicket> authenticationTicket in _ticketStore) {
                string authenticationTicketKey = authenticationTicket.Key;
                AuthenticationTicket authenticationTicketValue = authenticationTicket.Value;
                DateTimeOffset? expiresUtc = authenticationTicketValue.Properties.ExpiresUtc;
                if (expiresUtc != null && expiresUtc < DateTime.UtcNow) {
                    AuthenticationTicket authenticationTicketRemoved;
                    _ticketStore.TryRemove(authenticationTicketKey, out authenticationTicketRemoved);
                }
            }
            int ticketStoreCount = _ticketStore.Count;
            if (_logger.IsDebugEnabled) {
                _logger.DebugFormat("Cleanup on thread {0} with initial entry count of {1} and finally entry count of {2}",
                    Thread.CurrentThread.ManagedThreadId,
                    initialTicketsInStore,
                    ticketStoreCount);
            }
        }
    }
}