using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;

using Spring.Context.Support;
using Spring.Expressions;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Bietet die Möglichkeit der Authorisierung.
    ///     Nicht verwenden, wenn Seiten oder Teile davon gecached werden.
    /// </summary>
    /// <remarks>
    ///     Das Attribut ist als ActionFilter implementiert und wird damit später in der
    ///     Verarbeitungspipeline aufgerufen, als das Standard-Authorize Attribut. Prinzipiell
    ///     könnten davor andere ActionFilter aufgerufen werden. Diese dürfen dann noch kein Ergebnis
    ///     ausliefern, das einer Authorisierung bedarf.
    ///     Das Attribut darf nicht bei Seiten verwendet werden, die aus dem Cache kommen können,
    ///     da kein CacheValidateHandler implementiert ist und damit keine Authorisierung stattfinden kann.
    /// </remarks>
    public class PreAuthorizeAttribute : ActionFilterAttribute {
        private readonly string _actionExpression;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Web.Mvc.ActionFilterAttribute" /> class.
        /// </summary>
        public PreAuthorizeAttribute(string actionExpression) {
            _actionExpression = actionExpression;
        }

        public static IList<MethodParameter> GetMethodParameters(string parameterBlock, IDictionary<string, object> callingParameters) {
            List<MethodParameter> methodParameters = new List<MethodParameter>();
            string[] parameters = parameterBlock.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parameters.Length; i++) {
                if (parameters[i].Trim().StartsWith("#")) {
                    // Typ anhand der Methodenparameter bestimmen
                    string parameterName = parameters[i].Trim('#', ' ');
                    string rootParameterName = parameterName.Split('.').First();
                    object actionParameter = callingParameters[rootParameterName];
                    // Wenn es ein Property vom Parameter ist.
                    if (parameterName.Contains('.')) {
                        string parameterExpression = parameterName.Substring(parameterName.IndexOf('.') + 1);
                        actionParameter = ExpressionEvaluator.GetValue(actionParameter, parameterExpression);
                    }
                    methodParameters.Add(new MethodParameter()
                            { ParameterType = actionParameter.GetType(), ParameterName = parameterName, ParameterValue = actionParameter });
                } else if (parameters[i].Trim().StartsWith("'")) {
                    methodParameters.Add(new MethodParameter() { ParameterType = typeof(string), ParameterValue = parameters[i].Trim().Trim('\'') });
                } else {
                    throw new InvalidOperationException("Can't determine type from actionExpression parameters.");
                }
            }
            return methodParameters;
        }

        public static string GetParameterBlock(string actionExpression) {
            int startOfParameters = actionExpression.IndexOf('(');
            int endOfParameters = actionExpression.LastIndexOf(')');

            string parametersBlock = actionExpression.Substring(startOfParameters + 1, endOfParameters - startOfParameters - 1);
            return parametersBlock;
        }

        /// <summary>
        ///     Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // Service holen der für die Prüfungen zuständig ist.
            object securityService = GetSecurityService();

            // anhand der action eine Methode suchen die passen könnte -- Wenn es keine gibt dann Exception!
            IList<MethodParameter> parameters = GetParameterTypes(_actionExpression, filterContext);
            MethodInfo matchingMethod = FindMatchingMethod(_actionExpression, parameters, securityService);
            // Methode aufrufen und prüfen, ob ein true zurück kommt
            bool isAuthorized = (bool)matchingMethod.Invoke(securityService, parameters.Select(x => x.ParameterValue).ToArray());
            // Wenn nicht eine 403 Antwort erzeugen und an das Result hängen
            // Die Prüfung auf die Authentifizierung findet nur noch mal zur Sicherheit statt. Im Standardfall sollte das schon
            // geprüft sein.
            bool isAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated) {
                filterContext.Result = new HttpUnauthorizedResult();
            } else if (!isAuthorized) {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        public static string ParseMethodeName(string actionExpression) {
            int endOfMethodeName = actionExpression.IndexOf('(');
            if (endOfMethodeName <= 1) {
                throw new InvalidOperationException("Can't find a methode name.");
            }
            string methodeName = actionExpression.Substring(0, endOfMethodeName);
            return methodeName;
        }

        private MethodInfo FindMatchingMethod(string actionExpression, IList<MethodParameter> parameters, object securityService) {
            string methodName = ParseMethodeName(actionExpression);

            MethodInfo methodInfo = securityService.GetType().GetMethod(methodName, parameters.Select(x => x.ParameterType).ToArray());
            return methodInfo;
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private IList<MethodParameter> GetParameterTypes(string actionExpression, ActionExecutingContext filterContext) {
            string parametersBlock = GetParameterBlock(actionExpression);
            IList<MethodParameter> methodParameters = GetMethodParameters(parametersBlock, filterContext.ActionParameters);

            return methodParameters;
        }

        private ISecurityExpressionRoot GetSecurityService() {
            ISecurityExpressionRootFactory securityService =
                    ContextRegistry.GetContext().GetObject<ISecurityExpressionRootFactory>("webSecurityRootFactory");
            ISecurityExpressionRoot securityExpressionRoot = securityService.CreateSecurityExpressionRoot();
            return securityExpressionRoot;
        }
    }

    public class MethodParameter {
        private string _parameterName;
        private Type _parameterType;
        private object _parameterValue;

        public string ParameterName {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        public Type ParameterType {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        public object ParameterValue {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }

        /// <summary>
        ///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
        /// </summary>
        /// <returns>
        ///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
        /// </returns>
        /// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll.</param>
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            MethodParameter other = (MethodParameter)obj;

            if (!Equals(ParameterName, other.ParameterName)) {
                return false;
            }
            if (ParameterType != other.ParameterType) {
                return false;
            }
            if (!Equals(ParameterValue, other.ParameterValue)) {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Gibt eine Zeichenfolge zurück, die das aktuelle Objekt darstellt.
        /// </summary>
        /// <returns>
        ///     Eine Zeichenfolge, die das aktuelle Objekt darstellt.
        /// </returns>
        public override string ToString() {
            return string.Format("Name: {0}; Type: {1}, Wert: {2}", ParameterName, ParameterType, ParameterValue);
        }
    }
}