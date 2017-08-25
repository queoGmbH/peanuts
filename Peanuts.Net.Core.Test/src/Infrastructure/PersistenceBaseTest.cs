using Com.QueoFlow.Peanuts.Net.Core.Spring.Testing.NUnit;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure {
    [TestFixture]
    public class PersistenceBaseTest : AbstractTransactionalSpringContextTests {

        /// <summary>
        ///     Subclasses must implement this property to return the locations of their
        ///     config files. A plain path will be treated as a file system location.
        /// </summary>
        /// <value>
        ///     An array of config locations
        /// </value>
        protected override string[] ConfigLocations {
            get {
                return new[] {
                    "assembly://Peanuts.Net.Core.Test/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Database.Test.xml",
                    "assembly://Peanuts.Net.Core.Test/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Test.xml",
                    "assembly://Peanuts.Net.Core/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Service.xml",
                    "assembly://Peanuts.Net.Core/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Persistence.xml"
                };
            }
        }

    }
}