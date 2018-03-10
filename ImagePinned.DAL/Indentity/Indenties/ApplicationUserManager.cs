using ImagePinned.DAL.Indentity.Entietis;
using Microsoft.AspNet.Identity;

namespace ImagePinned.DAL.Indentity.Indenties
{
    public class ApplicationUserManager:UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
               : base(store)
        {
        }
    }
}
