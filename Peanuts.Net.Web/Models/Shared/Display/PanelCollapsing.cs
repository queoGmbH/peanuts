namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Display {
    
    
    public enum PanelCollapsing {
        

        /// <summary>
        /// Das Panel kann geöffnet und geschlossen werden und ist momentan geschlossen (nur Header sichtbar).
        /// </summary>
        IsCollapsed,

        /// <summary>
        /// Das Panel kann geöffnet und geschlossen werden und ist momentan geöffnet (alles sichtbar).
        /// </summary>
        CanCollapse,

        /// <summary>
        /// Das Panel kann NICHT geöffnet und/oder geschlossen werden.
        /// </summary>
        None

    }

}