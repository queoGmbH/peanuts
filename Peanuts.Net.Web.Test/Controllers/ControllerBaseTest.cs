using Com.QueoFlow.Peanuts.Net.Core.Spring.Testing.NUnit;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {

    /// <summary>
    /// Basisklasse für alle Testklassen zum Testen von Controllern.
    /// </summary>
    public abstract class ControllerBaseTest : AbstractTransactionalSpringContextTests {

        protected override string[] ConfigLocations {
            get {
                return new[] {
                    "assembly://Peanuts.Net.Core.Test/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Database.Test.xml",
                    "assembly://Peanuts.Net.Core.Test/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Test.xml",
                    "assembly://Peanuts.Net.Core/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Service.xml",
                    "assembly://Peanuts.Net.Core/Com.QueoFlow.Peanuts.Net.Core.Config/Spring.Persistence.xml",
                    "assembly://Peanuts.Net/Com.QueoFlow.Peanuts.Net.Web.Config/Spring.Controller.xml"
                };
            }
        }

    }
}