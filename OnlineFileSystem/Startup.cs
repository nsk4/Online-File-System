using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineFileSystem.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
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
