namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    public interface IGrantedAuthority {
        /// <summary>
        ///     Liefert die Berechtigung als string.
        /// </summary>
        /// <returns></returns>
        string Authority { get; }
    }
}