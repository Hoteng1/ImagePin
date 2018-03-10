using ImagePinned.BLL.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.BLL.Identity.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserForIdentity userDto);
        Task<ClaimsIdentity> Authenticate(UserForIdentity userDto);
        Task SetInitialData(UserForIdentity adminDto,List<string> roles);
        Task DeleteUserAsync(UserForIdentity userDto);
        Task ChangePasswordAsync(UserForIdentity userDto, string NewPassword);
    }
}
