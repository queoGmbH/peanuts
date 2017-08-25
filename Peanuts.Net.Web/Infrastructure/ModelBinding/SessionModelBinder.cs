using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    /// <summary>
    ///     Modelbinder, der ein Model aus der Session lädt, wenn es nicht über den Request gebunden werden kann.
    /// 
    ///     Wird das Model über den Request gebunden, werden die Daten direkt an der Session gespeichert. 
    ///     Dazu wird für alle Properties des Models (bindingContext.PropertyMetaData) der Original-Wert (nicht der gebundene Wert!!!) an der Session hinterlegt. 
    /// 
    ///     Kann das Model nicht über den Request gebunden werden, wird versucht das Model anhand der Werte aus der Session zu binden.
    ///     Dazu wird kurzzeitig der original ValueProvider des BindingContext ausgetauscht und durch einen eigenen ValueProvider ersetzt, der über die Werte aus der Session (nur für die ModelProperties) verfügt.
    /// </summary>
    public class SessionModelBinder : DefaultModelBinder {
        /// <summary>
        ///     Bindet das Modell, indem der angegebene Controllerkontext und Bindungskontext verwendet werden.
        /// </summary>
        /// <returns>
        ///     Das gebundene Objekt.
        /// </returns>
        /// <param name="controllerContext">
        ///     Der Kontext, innerhalb dessen der Controller funktioniert.Die Kontextinformationen
        ///     enthalten den Controller, den HTTP-Inhalt, den Anforderungskontext und die Weiterleitungsdaten.
        /// </param>
        /// <param name="bindingContext">
        ///     Der Kontext, in dem das Modell gebunden ist.Der Kontext schließt Informationen ein, z. B.
        ///     das Modellobjekt, den Modellnamen, den Modelltyp, den Eigenschaftenfilter und den Wertanbieter.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">Der <paramref name="bindingContext " />-Parameter ist null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            /*Model deklarieren*/
            object boundModel;

            /* Prüfen ob aus dem Request oder aus der Session gebunden werden soll. Wenn die Session NULL ist, dann DefaultBinding */
            string prefix = bindingContext.ModelName;
            if (bindingContext.ValueProvider.ContainsPrefix(prefix) || controllerContext.HttpContext.Session == null) {
                /*Aus dem Request binden*/
                boundModel = base.BindModel(controllerContext, bindingContext);

                /*Gebundene Eigenschaften an die Session schreiben*/
                WritePropertyValuesToSession(controllerContext, bindingContext);
            } else {
                /*Aus der Session binden*/

                /*Die Werte aus der Session, werden in einen eigenen ValueProvider gelegt, der dann für das Binding verwendet wird.*/
                var sessionValueProvider = CreateSessionValueProvider(controllerContext, bindingContext);

                /*ValueProvider am BindingContext merken und austauschen.*/
                var originalValueProvider = bindingContext.ValueProvider;
                bindingContext.ValueProvider = sessionValueProvider;

                /*DefaultBinding mit ausgetauschtem ValueProvider*/
                /*TODO: Es wurde bisher keine Möglichkeit gefunden, den SessionValueProvider zur Liste der Value-Provider hinzuzufügen. 
                 * Evtl. ist das der bessere Ansatz, als das Austauschen.*/
                boundModel = base.BindModel(controllerContext, bindingContext);
                
                /*ValueProvider wieder zurücksetzen.*/
                bindingContext.ValueProvider = originalValueProvider;
            }

            return boundModel;
        }

        /// <summary>
        /// Die Werte für die Binding des Models an die Session schreiben.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        public static void WritePropertyValuesToSession(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            if (controllerContext.HttpContext.Session != null) {
                SessionValueProvider sessionValueProvider = SessionValueProvider.FromBindingContext(bindingContext);
                SessionPersister.WriteToSession(controllerContext.HttpContext.Session, sessionValueProvider);
            }
        }

        

        /// <summary>
        /// Erzeugt einen eigenen ValueProvider mit den Werten für die ModelProperties aus der Session.
        /// </summary>
        private SessionValueProvider CreateSessionValueProvider(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            return SessionValueProvider.FromSession(controllerContext, bindingContext);
        }
        
    }
}