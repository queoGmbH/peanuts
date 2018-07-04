using System;
using System.Reflection;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public static class VersionHelper {

        /// <summary>
        /// Ruft die aktuelle Version der Anwendung (entscheidend ist die Version der Webanwendung) ab.
        /// </summary>
        /// <returns></returns>
        public static Version GetCurrentApplicationVersion() {
            Assembly assembly = Assembly.GetAssembly(typeof(MvcApplication));
            return assembly.GetName().Version;
        }
    }
}