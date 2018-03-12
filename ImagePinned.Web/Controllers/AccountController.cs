using ImagePinned.BLL.Interface;
using ImagePinned.Web.Controllers;
using ImagePinned.BLL.Identity;
using ImagePinned.BLL.Identity.Infrastructure;
using ImagePinned.BLL.Identity.Interfaces;
using ImagePinned.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ListShop.Web.Validation;

namespace ImagePinned.Web.Controllers
{
    [ServerException]
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private IService service
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IService>();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            
            
            if (ModelState.IsValid)
            {
                UserForIdentity userForIdentity = new UserForIdentity { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userForIdentity);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            
            if (ModelState.IsValid)
            {
                UserForIdentity userForIdentity = new UserForIdentity
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Login,
                    Role = "user"
                    
                };
                OperationDetails operationDetails = await UserService.Create(userForIdentity);
                if (operationDetails.Succedeed)
                {
                    service.CreateUser(new BLL.EntitesDTO.UserDTO { Email = model.Login, Login = model.Email});
                    ClaimsIdentity claim = await UserService.Authenticate(userForIdentity);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return View("SuccessRegister");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
        private async Task SetInitialDataAsync()
        {
            
            await UserService.SetInitialData(new UserForIdentity
            {
                Email = "Admin@mail.ru",
                Name = "Admin",
                UserName="Admin",
                Password = "5335482",
                Role="admin"
                
            }, new List<string> { "user", "admin" });
           
        }


    }
}