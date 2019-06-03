using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(S_FOOD.Startup))]
namespace S_FOOD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
