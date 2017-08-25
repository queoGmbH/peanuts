using System.Web.Optimization;

namespace Com.QueoFlow.Peanuts.Net.Web
{
    public class BundleConfig
    {
        // Weitere Informationen zu Bundling finden Sie unter "http://go.microsoft.com/fwlink/?LinkId=301862"
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*Da die Styles und Scripts per Node minifiziert und gebundled werden, ist das hier deaktiviert und die Dateien werden im Layout direkt eingebunden*/
            //bundles.Add(new ScriptBundle("~/Scripts").Include("~/Content/dist/javascripts/app.min.js"));
            //bundles.Add(new StyleBundle("~/Styles").Include("~/Content/dist/stylesheets/app.min.css"));

        }
    }
}
