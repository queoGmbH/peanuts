using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding;

namespace Com.QueoFlow.Peanuts.Net.Web {
    public class ModelBinderConfig {
        /// <summary>
        ///     Registriert die im System zu verwendenden ModelBinder.
        ///     Dazu müssen die ModelBinder der übergebenen ModelBinder-Liste hinzugefügt werden.
        /// </summary>
        /// <param name="modelBinders">Liste/Dictionary der die ModelBinder hinzugefügt werden müssen.</param>
        public static void RegisterModelBindings(ModelBinderDictionary modelBinders) {
            ModelBinderProviders.BinderProviders.Add(new EntityModelBinderProvider());
            //ModelBinderProviders.BinderProviders.Add(new InvariantCultureModelBinderProvider(typeof(double),
            //    typeof(double?),
            //    typeof(decimal),
            //    typeof(decimal?)));
            ModelBinderProviders.BinderProviders.Add(new FileInfoModelBinderProvider());
            modelBinders.Add(typeof(ISessionBindable), new SessionModelBinder());
        }
    }
    
}