using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TeamBuilder.Web.Startup))]

namespace TeamBuilder.Web
{
    using TeamBuilder.Clients.Infrastructure.Identity;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            NinjectConfig.Configure(app);
            StartupAuth authStartup = new StartupAuth();
            authStartup.ConfigureAuth(app);
        }
    }
}
