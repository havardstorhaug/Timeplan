using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Timeplan.Startup))]
namespace Timeplan
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
