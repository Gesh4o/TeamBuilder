using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TeamBuilder.Web.Startup))]

namespace TeamBuilder.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
