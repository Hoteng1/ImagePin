using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.AspNet.Identity;
using ListShop.BLL.Identity.Service;
using ListShop.BLL.Identity.Interfaces;

[assembly: OwinStartup(typeof(ListShop.Web.App_Start.Startup))]

namespace ListShop.Web.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();
        
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.CreatePerOwinContext<ImagePinned.BLL.Interface.IService>(CreateSerivce);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService("DefaultConnection");
        }

        private ImagePinned.BLL.Interface.IService CreateSerivce()
        {
            return serviceCreator.CreateService("DefaultConnection");
        }
    }
}