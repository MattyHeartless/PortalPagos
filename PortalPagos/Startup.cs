using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortalPagos.Startup))]
namespace PortalPagos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
