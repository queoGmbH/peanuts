namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure {
    /// <summary>
    /// Basisklasse für alle Tests auf Serviceebene.
    /// </summary>
    /// <remarks>
    /// Eigentlich nicht mehr erforderlich, aber damit es so aussieht wie man es gewohnt ist.
    /// Damit der SpringContext für die Tests sinnvoll gecacht werden kann, darf es nur eine Konfiguration geben,
    /// und nicht wir bisher unterschiedliche in der PersistenceBaseTest und ServiceBaseTest.
    /// </remarks>
    public class ServiceBaseTest:PersistenceBaseTest {
        
    }
}