namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement {
    public interface IMessageProvider {
        /// <summary>
        ///     Rendert eine Mailmessage aus dem angegebenen Template und verwendet dabei die Daten aus dem Model.
        /// </summary>
        /// <param name="templateName">Name des Templates</param>
        /// <param name="model">Daten für das Template</param>
        /// <returns></returns>
        string RenderMessage(string templateName, ModelMap model);
    }
}