using ListShop.DAL.Indentity.Entietis;
using Microsoft.AspNet.Identity;

namespace ListShop.DAL.Indentity.Indenties
{
    public class ApplicationUserManager:UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
               : base(store)
        {
        }
    }
}
