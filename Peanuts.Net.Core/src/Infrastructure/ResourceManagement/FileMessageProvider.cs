using System.Globalization;
using System.IO;
using System.Threading;

using Common.Logging;

using Spring.Context.Support;
using Spring.Core.IO;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement {
    /// <summary>
    ///     Die Klasse unterstützt das Laden eines Templates aus einer Datei.
    /// </summary>
    /// <remarks>
    ///     Der Name der Datei besteht aus dem templateName an den .template angehangen wird.
    ///     Es ist der <code>ResourceRelativePath</code> zu setzten, der angibt, wo die Templates relativ zum RootDirectory
    ///     liegen.
    ///     Die Culture für die Ressource wird aus dem Thread genommen <code>Thread.CurrentThread.CultureInfo</code>. Existiert
    ///     kein
    ///     culturespezifisches Template wird die Cultur weiter verallgemeinert bis der Fallback (ohne Cultureangabe) erreicht
    ///     wird. Existiert auch
    ///     dafür kein Template, wird eine Exception geworfen.
    ///     Beispiel für den Ablauf:
    ///     templateName = TestMessage, CulturInfo im Thread: DE-CH
    ///     1. TestMessage.de-ch.template
    ///     2. TestMessage.de.template
    ///     3. TestMessage.template
    /// </remarks>
    public class FileMessageProvider : IMessageProvider {
        private readonly ILog _log = LogManager.GetLogger(typeof(FileMessageProvider));

        public string ResourceRelativePath { get; set; }

        /// <summary>
        ///     Rendert eine Mailmessage aus dem angegebenen Template und verwendet dabei die Daten aus dem Model.
        /// </summary>
        /// <param name="templateName">Name des Templates</param>
        /// <param name="model">Daten für das Template</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Falls das Template nicht gefunden wird.</exception>
        public string RenderMessage(string templateName, ModelMap model) {
            _log.DebugFormat("Render message für das Template {0}.", templateName);
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            string template = LoadMailMessageTemplate(templateName, cultureInfo);
            Templater templater = new Templater(cultureInfo);
            string evaluatedTemplate = templater.FormatTemplate(template, model);
            return evaluatedTemplate;
        }

        private IResource FindResource(string templateName, CultureInfo cultureInfo) {
            string cultureName = cultureInfo.Name;
            string cultureInfix = string.IsNullOrEmpty(cultureName) ? "" : "." + cultureName;
            string resourcePath = "file://~" + ResourceRelativePath + "/" + templateName + cultureInfix + ".template";
            IResource resource = ContextRegistry.GetContext().GetResource(resourcePath);
            _log.DebugFormat("Versuche die Resource {0} zu laden.", resource.Uri.AbsoluteUri);

            if (!resource.Exists) {
                if (string.IsNullOrEmpty(cultureInfo.Name)) {
                    _log.ErrorFormat("Die Ressource {0} wurde nicht gefunden.", resource.Uri.AbsoluteUri);
                    throw new FileNotFoundException(string.Format("Die Ressource {0} wurde nicht gefunden", resource.Uri.AbsoluteUri));
                }
                resource = FindResource(templateName, cultureInfo.Parent);
            }
            return resource;
        }

        private string LoadMailMessageTemplate(string templateName, CultureInfo cultureInfo) {
            IResource resource = FindResource(templateName, cultureInfo);
            string template;
            using (StreamReader reader = new StreamReader(resource.InputStream)) {
                template = reader.ReadToEnd();
            }
            return template;
        }
    }
}