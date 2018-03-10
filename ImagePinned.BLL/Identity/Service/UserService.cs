using ImagePinned.BLL.Identity.Infrastructure;
using ImagePinned.BLL.Identity.Interfaces;
using ImagePinned.DAL.Indentity.Entietis;
using ImagePinned.DAL.Indentity.Intefaces;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImagePinned.BLL.Identity.Service
{
    public class UserService :IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task DeleteUserAsync(UserForIdentity userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            ClientProfile clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
            Database.ClientManager.Delete(clientProfile);
            var result = await Database.UserManager.DeleteAsync(user);
            if (result.Errors.Count() > 0)
                 new OperationDetails(false, result.Errors.FirstOrDefault(), "");
           
            await Database.SaveAsync();
        }

        public async Task ChangePasswordAsync(UserForIdentity userDto , string NewPassword)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            ClientProfile clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
            Database.ClientManager.Delete(clientProfile);
            var result = await Database.UserManager.DeleteAsync(user);
            if (result.Errors.Count() > 0)
                new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            await Database.SaveAsync();

            user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
            result = await Database.UserManager.CreateAsync(user, NewPassword);
            
            await Database.UserManager.AddToRoleAsync(user.Id, "user");
            clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
            Database.ClientManager.Create(clientProfile);
            await Database.SaveAsync();
        }

        public async Task<OperationDetails> Create(UserForIdentity userDto)
        {
            
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // создаем профиль клиента
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserForIdentity userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserForIdentity adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

      
    }
}
