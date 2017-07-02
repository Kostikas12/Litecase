using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Litecase.Startup))]
namespace Litecase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
