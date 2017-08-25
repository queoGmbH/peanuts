using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

using Spring.Expressions;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    public class AuthorizationManager {
        private readonly Dictionary<string, IList<string>> _rolesToPermissions = new Dictionary<string, IList<string>>() {
            {
                Roles.Administrator, new List<string>() { "proposedUser_read", "proposedUser_update", "proposedUser_create", "proposedUser_create" }
            }, {
                Roles.Member, new List<string>()
            }
        };

        private readonly User _user;

        public AuthorizationManager() {
        }

        public AuthorizationManager(User user) {
            _user = user;
        }

        public IList<IGrantedAuthority> GetAuthorities() {
            // TODO: Da es einen leeren Konstruktor gibt, kann es sein, dass kein Nutzer vorhanden ist. 
            // Dann werden keine Berechtigungen gewährt.
            if (_user == null) {
                return new List<IGrantedAuthority>();
            }
            return GetAuthorities(_user.Roles);
        }

        public IList<IGrantedAuthority> GetAuthorities(string role) {
            List<string> roles = new List<string>() { role };
            IList<IGrantedAuthority> grantedAuthorities = GetAuthorities(roles);
            return grantedAuthorities;
        }

        /// <summary>
        ///     Liefert alle <see cref="IGrantedAuthority" /> der angebenen Rolle für den Nutzer im AuthotizationManager.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public IList<IGrantedAuthority> GetAuthorities(IList<string> roles) {
            Require.NotNull(roles, nameof(roles));

            List<IGrantedAuthority> grantedAuthorities = new List<IGrantedAuthority>();
            foreach (string role in roles) {
                if (_rolesToPermissions.ContainsKey(role)) {
                    // Rolle als Authority hinzufügen
                    grantedAuthorities.Add(new SimpleGrantedAuthority(role));
                    foreach (string permission in _rolesToPermissions[role]) {
                        if (AttributeExpressionParser.IsExpression(permission)) {
                            SimpleGrantedAuthority authorityForExpression = GetAuthorityForExpression(permission);
                            if (!grantedAuthorities.Contains(authorityForExpression)) {
                                grantedAuthorities.Add(authorityForExpression);
                            }
                        } else {
                            SimpleGrantedAuthority authority = new SimpleGrantedAuthority(permission);
                            if (!grantedAuthorities.Contains(authority)) {
                                grantedAuthorities.Add(authority);
                            }
                        }
                    }
                }
            }
            return grantedAuthorities;
        }

        private SimpleGrantedAuthority GetAuthorityForExpression(string permission) {
            // Wenn es keinen Nutzer gibt, kann nichts evaluiert werden, also gibt es auch keine Berechtigung.
            if (_user == null) {
                return null;
            }
            string expression = AttributeExpressionParser.Parse(permission);
            object evaluatedExpression = ExpressionEvaluator.GetValue(_user, expression);
            if (evaluatedExpression == null) {
                // Es konnte kein Wert bestimmt werden, also gibt es auch keine Berechtigung.
                return null;
            }
            string first = permission.Split('{').First();
            string last = permission.Split('}').Last();
            SimpleGrantedAuthority simpleGrantedAuthority = new SimpleGrantedAuthority(first + evaluatedExpression + last);
            return simpleGrantedAuthority;
        }

        /*
         public IList<IGrantedAuthority> GetAuthorities(IList<string> roles) {
            Require.NotNull(roles, nameof(roles));

            List<IGrantedAuthority> grantedAuthorities = new List<IGrantedAuthority>();
            foreach (string role in roles) {
                if (_rolesToPermissions.ContainsKey(role)) {
                    // Rolle als Authority hinzufügen
                    grantedAuthorities.Add(new SimpleGrantedAuthority(role));
                    foreach (string permission in _rolesToPermissions[role]) {
                        if (permission.Contains("{")) {
                            // Auswerten
                            if (_user != null) {
                                int startIndex = permission.IndexOf("{");
                                int endIndex = permission.IndexOf("}");
                                string expression = permission.Substring(startIndex + 1, endIndex - startIndex - 1);
                                object value = ExpressionEvaluator.GetValue(_user, expression);
                                string first = permission.Split('{').First();
                                string last = permission.Split('}').Last();
                                SimpleGrantedAuthority simpleGrantedAuthority = new SimpleGrantedAuthority(first + value + last);
                                if (!grantedAuthorities.Contains(simpleGrantedAuthority)) {
                                    grantedAuthorities.Add(simpleGrantedAuthority);
                                }
                            }
                        } else {
                            SimpleGrantedAuthority authority = new SimpleGrantedAuthority(permission);
                            if (!grantedAuthorities.Contains(authority)) {
                                grantedAuthorities.Add(authority);
                            }
                        }
                    }
                }
            }
            return grantedAuthorities;
        }
         */
    }
}