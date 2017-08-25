using System.Collections.Generic;
using System.Linq;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils {
    public static class ListHelper {
        /// <summary>
        ///     Überprüft, ob zwei Listen AreEquivalent sind, ohne die Reihenfolge der Elemente zu beachten.
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool AreEquivalent<TList>(IList<TList> list1, IList<TList> list2) {
            if (object.Equals(list1, list2)) {
                return true;
            }

            if (list1 == null || list2 == null) {
                return false;
            }

            return list1.Count == list2.Count && !list1.Except(list2).Any();
        }
    }
}