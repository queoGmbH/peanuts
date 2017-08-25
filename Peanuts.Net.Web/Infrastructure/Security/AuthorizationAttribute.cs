using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;

using Spring.Context.Support;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Erweiterung zum Autorize Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter {
        private readonly string[] _roles;

        /// <summary>
        ///     Nur die Prüfung, ob der Nutzer authentifiziert ist.
        /// </summary>
        public AuthorizationAttribute() {
        }

        /// <summary>
        ///     Rolle, welche der Nutzer haben muss.
        /// </summary>
        /// <param name="role"></param>
        public AuthorizationAttribute(string role) {
            _roles = new[] { role };
        }

        /// <summary>
        ///     Liste von Rollen, von denen dem Nutzer mindestens eine zugewiesen sein muss.
        /// </summary>
        /// <param name="roles"></param>
        public AuthorizationAttribute(params string[] roles) {
            _roles = roles;
        }

        /// <summary>
        ///     Wird aufgerufen, wenn eine Autorisierung erforderlich ist.
        /// </summary>
        /// <param name="filterContext">Der Filterkontext.</param>
        public virtual void OnAuthorization(AuthorizationContext filterContext) {
            if (filterContext == null) {
                throw new ArgumentNullException("filterContext");
            }
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext)) {
                throw new InvalidOperationException("Can't use this attribute with ChildActionCache.");
            }
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)) {
                return;
            }
            if (AuthorizeCore(filterContext.HttpContext)) {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(CacheValidateHandler, null);
            } else {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        /// <summary>
        ///     Stellt beim Überschreiben einen Einstiegspunkt für benutzerdefinierte Autorisierungs-Prüfungen bereit.
        /// </summary>
        /// <returns>
        ///     True, wenn der Benutzer autorisiert ist, andernfalls False.
        /// </returns>
        /// <param name="httpContext">
        ///     Der HTTP-Kontext, der alle HTTP-spezifischen Informationen über eine einzelne
        ///     HTTP-Anforderung kapselt.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">Der <paramref name="httpContext" />-Parameter ist null.</exception>
        protected virtual bool AuthorizeCore(HttpContextBase httpContext) {
            if (httpContext == null) {
                throw new ArgumentNullException("httpContext");
            }
            
            if (!httpContext.User.Identity.IsAuthenticated) {
                /*Es ist kein Nutzer angemeldet*/
                return false;
            }

            ISecurityExpressionRoot securityExpressionRoot = GetSecurityContext();

            /* Wenn keine Rollen angegeben sind, muss der Nutzer nur authentifiziert sein. */
            if (_roles == null || _roles.Length == 0) {
                return true;
            }

            return securityExpressionRoot.HasAnyRole(_roles);
        }

        /// <summary>
        ///     Verarbeitet HTTP-Anforderungen, deren Autorisierung nicht erfolgreich war.
        /// </summary>
        /// <param name="filterContext">
        ///     Kapselt die Informationen zum Verwenden des
        ///     <see cref="T:System.Web.Mvc.AuthorizeAttribute" />-Objekts.Das <paramref name="filterContext" />-Objekt enthält den
        ///     Controller, den HTTP-Kontext, den Anforderungskontext, das Aktionsergebnis und die Routen-Daten.
        /// </param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) {
                // filterContext.Result = new HttpUnauthorizedResult();
                throw new HttpException((int)HttpStatusCode.Unauthorized, "Nicht angemeldet.");
            } else {
                // filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                throw new HttpException((int)HttpStatusCode.Forbidden, "Nicht autorisiert.");
            }
        }

        /// <summary>
        ///     Wird aufgerufen, wenn das Cache-Modul eine Autorisierung anfordert.
        /// </summary>
        /// <returns>
        ///     Ein Verweis auf den Validierungs-Status.
        /// </returns>
        /// <param name="httpContext">
        ///     Der HTTP-Kontext, der alle HTTP-spezifischen Informationen über eine einzelne
        ///     HTTP-Anforderung kapselt.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">Der <paramref name="httpContext" />-Parameter ist null.</exception>
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext) {
            if (httpContext == null) {
                throw new ArgumentNullException("httpContext");
            }
            return !AuthorizeCore(httpContext) ? HttpValidationStatus.IgnoreThisRequest : HttpValidationStatus.Valid;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus) {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        private ISecurityExpressionRoot GetSecurityContext() {
            ISecurityExpressionRootFactory securityExpressionRootFactory = ContextRegistry.GetContext().GetObject<ISecurityExpressionRootFactory>();
            ISecurityExpressionRoot securityExpressionRoot = securityExpressionRootFactory.CreateSecurityExpressionRoot();
            return securityExpressionRoot;
        }
        
    }
}