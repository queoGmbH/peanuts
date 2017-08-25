using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;

using Spring.Context.Support;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class MvcSecurityExtension {
        private HtmlHelper helper;

        public MvcSecurityExtension(HtmlHelper helper) {
            this.helper = helper;
        }

        public bool IsAuthenticated() {
            ISecurityExpressionRoot securityExpressionRoot = GetSecurityExpressionRoot();
            return securityExpressionRoot.IsAuthenticated();
        }

        public bool HasRole(string role) {
            ISecurityExpressionRoot securityExpressionRoot = GetSecurityExpressionRoot();
            return securityExpressionRoot.HasRole(role);
        }

        public bool HasAnyRole(params string[] roles) {
            ISecurityExpressionRoot securityExpressionRoot = GetSecurityExpressionRoot();
            return securityExpressionRoot.HasAnyRole(roles);
        }

        private ISecurityExpressionRoot GetSecurityExpressionRoot() {
            ISecurityExpressionRootFactory securityExpressionRootFactory =
                    ContextRegistry.GetContext().GetObject<ISecurityExpressionRootFactory>();
            ISecurityExpressionRoot securityExpressionRoot = securityExpressionRootFactory.CreateSecurityExpressionRoot();
            return securityExpressionRoot;
        }
    }
}