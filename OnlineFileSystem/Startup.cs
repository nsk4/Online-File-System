using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineFileSystem.Startup))]
namespace OnlineFileSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
