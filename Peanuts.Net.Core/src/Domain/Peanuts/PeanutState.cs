namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    /// Listet mögliche Status eines Peanuts auf.
    /// </summary>
    public enum PeanutState {

        /// <summary>
        /// Der Peanut befindet sich in der Planung.
        /// Bsp.: Koch-Peanut => "Rezept-Findung und Mengenplanung"
        /// </summary>
        Scheduling,

        /// <summary>
        /// Der Peanut wurde abgesagt.
        /// Bsp.: Koch-Peanut => "Kochen wurde abgesagt"
        /// </summary>
        Canceled,

        /// <summary>
        /// Die Planung des Peanuts ist abgeschlossen.
        /// Bsp.: Koch-Peanut => "Rezept-Findung und Mengenplanung"
        /// </summary>
        SchedulingDone,

        /// <summary>
        /// Die Anschaffung der Voraussetzungen hat begonnen.
        /// Bsp.: Koch-Peanut => "Einkauf gestartet"
        /// </summary>
        PurchasingStarted,

        /// <summary>
        /// Die Anschaffung der Voraussetzungen ist abgeschlossen.
        /// Bsp.: Koch-Peanut => "Einkauf abgeschlossen"
        /// </summary>
        PurchasingDone,


        /// <summary>
        /// Der Peanut befindet sich in der Herstellung bzw. im Zusammenbau.
        /// Bsp.: Koch-Peanut => "Kochen"
        /// </summary>
        Assembling,

        /// <summary>
        /// Der Peanut wurde gestartet.
        /// Bsp.: Koch-Peanut => "Es gibt essen"
        /// </summary>
        Started,

        /// <summary>
        /// Der Peanut wurde durchgeführt
        /// Bsp.: Koch-Peanut => "Essen beendet"
        /// </summary>
        Realized
    }
}