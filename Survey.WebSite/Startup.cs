using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Survey.WebSite.Startup))]
namespace Survey.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
