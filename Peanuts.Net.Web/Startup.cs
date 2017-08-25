using Com.QueoFlow.Peanuts.Net.Web;

using Microsoft.Owin;

using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Com.QueoFlow.Peanuts.Net.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
