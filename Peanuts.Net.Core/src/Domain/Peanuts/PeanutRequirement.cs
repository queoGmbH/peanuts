namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    ///     Bildet eine Voraussetzung ab, die zur Erfüllung/Durchführung eines Peanuts benötigt wird.
    /// </summary>
    public class PeanutRequirement {
        private readonly string _name;

        private readonly double _quantity;
        private readonly string _unit;
        private readonly string _url;

        public PeanutRequirement() {
        }

        public PeanutRequirement(string name, double quantity, string unit, string url) {
            _quantity = quantity;
            _name = name;
            _unit = unit;
            _url = url;
        }

        /// <summary>
        ///     Ruft den Namen dieser Voraussetzung ab.
        /// </summary>
        public virtual string Name {
            get { return _name; }
        }

        /// <summary>
        ///     Ruft die dieser Voraussetzung ab.
        /// </summary>
        public virtual double Quantity {
            get { return _quantity; }
        }

        /// <summary>
        ///     Ruft eine zusammengesetzt ZUeichenkette aus Menge und Einheit der Voraussetzung ab.
        /// </summary>
        public virtual string QuantityAndUnit {
            get { return string.Format("{0} {1}", Quantity, Unit); }
        }

        /// <summary>
        ///     Ruft die Einheit der Menge dieser Voraussetzung ab.
        /// </summary>
        public virtual string Unit {
            get { return _unit; }
        }

        /// <summary>
        ///     Ruft eine URL ab, auf der die Voraussetzung beschrieben oder ein Bild o.ä. enthalten ist.
        /// </summary>
        public virtual string Url {
            get { return _url; }
        }

        public virtual RequirementDto GetDto() {
            return new RequirementDto(_name, _quantity, _unit, _url);
        }
    }
}