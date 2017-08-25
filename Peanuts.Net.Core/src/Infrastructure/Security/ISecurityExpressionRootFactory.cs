namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    /// <summary>
    ///     Schnittstelle für die Factory zum Erzeugen einer <see cref="ISecurityExpressionRoot" />.
    /// </summary>
    public interface ISecurityExpressionRootFactory {
        /// <summary>
        ///     Erzeugt eine Implementierung von <see cref="ISecurityExpressionRoot" />.
        /// </summary>
        /// <returns></returns>
        ISecurityExpressionRoot CreateSecurityExpressionRoot();
    }
}