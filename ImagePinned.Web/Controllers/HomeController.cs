using ImagePinned.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ImagePinned.Web.Models;
using Ninject;
using ImagePinned.BLL.EntitesDTO;
using Microsoft.Owin.Security;
using ImagePinned.BLL.Identity.Service;
using ImagePinned.BLL.Identity.Infrastructure;
using ImagePinned.BLL.Identity;
using System.Threading.Tasks;
using ImagePinned.BLL.Identity.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;

namespace ImagePinned.Web.Controllers
{
    public class HomeController : Controller
    {
        private IService service
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
       
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {

            return View();
        }



        public PartialViewResult UserLogin()
        {
            if (AuthenticationManager.User.Identity.IsAuthenticated)
            {


                return PartialView(new UserViews { Email = AuthenticationManager.User.Identity.Name, Role = AuthenticationManager.User.IsInRole("admin") });
            }
            return PartialView(new UserViews { Email = null });

        }

        public ActionResult Rating()
        {


            return View();
        }

        private IEnumerable<ImageModel> Sorted(IEnumerable<ImageModel> list)
        {
            List<ImageModel> result = new List<ImageModel>(list.OrderByDescending(s => s.CountLike));
            int i = 1;
            foreach (var item in result)
            {
                item.Id = i;
                i++;
            }
            return result;

        }

        public PartialViewResult RatingImage()
        {
            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);

            return PartialView(Sorted(images));
        }

        public PartialViewResult RatingUser()
        {
            List<UserDTO> list = new List<UserDTO>(service.UserGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserRating>());

            var user =
                Mapper.Map<IEnumerable<UserDTO>, List<UserRating>>(list);

            return PartialView(SortedUser(user));
        }

        private IEnumerable<UserRating> SortedUser(IEnumerable<UserRating> list)
        {
            List<UserRating> result = new List<UserRating>(list.OrderByDescending(s => s.Count_Like));
            int i = 1;
            foreach (var item in result)
            {
                item.Id = i;
                i++;
            }
            return result;

        }

        public ActionResult Profiles()
        {
            UserDTO userDTO = service.GetUser(AuthenticationManager.User.Identity.Name);
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserRating>());

            var user =
                Mapper.Map<UserDTO, UserRating>(userDTO);

            return View(user);
        }



        // [HttpGet]
        public PartialViewResult Pins()
        {
            List<PinDTO> list = new List<PinDTO>(service.GetNotPinAll(AuthenticationManager.User.Identity.Name));
            Mapper.Initialize(cfg => cfg.CreateMap<PinDTO, PinView>());

            var pins =
                Mapper.Map<IEnumerable<PinDTO>, List<PinView>>(list);



            return PartialView(pins);
        }


        public ActionResult AddPin(string Pin)
        {
            service.AddPrefer(Pin, AuthenticationManager.User.Identity.Name);
            UserDTO userDTO = service.GetUser(AuthenticationManager.User.Identity.Name);
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserRating>());

            var user =
                Mapper.Map<UserDTO, UserRating>(userDTO);

            return View("Profiles", user);
        }

        public ActionResult DeletePin(string Pin)
        {
            service.DeletePrefer(Pin, AuthenticationManager.User.Identity.Name);
            UserDTO userDTO = service.GetUser(AuthenticationManager.User.Identity.Name);
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserRating>());

            var user =
                Mapper.Map<UserDTO, UserRating>(userDTO);

            return View("Profiles", user);
        }

        public PartialViewResult Category()
        {
            List<PinDTO> list = new List<PinDTO>(service.GetPinAll(AuthenticationManager.User.Identity.Name));
            Mapper.Initialize(cfg => cfg.CreateMap<PinDTO, PinView>());

            var pins =
                Mapper.Map<IEnumerable<PinDTO>, List<PinView>>(list);



            return PartialView(pins);

        }

        [Authorize]
        public ActionResult AdminPanel()
        {

            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);

            return View(images);





        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            ImageDTOView imageDTOView = service.GetImage(id);
            if (imageDTOView != null && (imageDTOView.Own_Name == AuthenticationManager.User.Identity.Name || AuthenticationManager.User.IsInRole("admin")))
            {
                service.Delete(id);
               
            }

            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);

            if (AuthenticationManager.User.IsInRole("admin"))
            {
                return View("AdminPanel", images);
            }

            UserDTO userDTO = service.GetUser(AuthenticationManager.User.Identity.Name);
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserRating>());

            var user =
                Mapper.Map<UserDTO, UserRating>(userDTO);

            return View("Profiles", user);



        }

        [Authorize]
        public ActionResult Edit(int id)
        {

            ImageDTOView imageDTOView = service.GetImage(id);

            if (AuthenticationManager.User.IsInRole("admin") || imageDTOView.Own_Name.Equals(AuthenticationManager.User.Identity.Name))
            {

                Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
                var images =
                  Mapper.Map<ImageDTOView, ImageModel>(imageDTOView);

                return View(images);
            }
            return View("Index");
        }


        [HttpPost]
        public ActionResult Edit(ImageModel model)
        {


            Mapper.Initialize(cfg => cfg.CreateMap<ImageModel, ImageDTOView>());
            var image =
              Mapper.Map<ImageModel, ImageDTOView>(model);

            service.Update(image);
            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
            if (AuthenticationManager.User.IsInRole("admin"))
            {
                return View("AdminPanel", images);
            }
            UserDTO userDTO = service.GetUser(AuthenticationManager.User.Identity.Name);
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserRating>());

            var user =
                Mapper.Map<UserDTO, UserRating>(userDTO);

            return View("Profiles", user);

        }

        [Authorize]
        public ActionResult Content()
        {
            List<ImageDTOView> list = new List<ImageDTOView>(service.GetImage(AuthenticationManager.User.Identity.Name));
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);

            return View(images);
        }

        [Authorize]
        public ActionResult ManagedPin()
        {
            if (AuthenticationManager.User.IsInRole("admin"))
            {
                List<PinDTOView> list = new List<PinDTOView>(service.GetPins());
                Mapper.Initialize(cfg => cfg.CreateMap<PinDTOView, PinModel>());
                var pins =
                   Mapper.Map<IEnumerable<PinDTOView>, List<PinModel>>(list);

                return View(pins);
            }

            return View("Index");
        }

        [Authorize]
        public ActionResult PinDelete(int id)
        {
            if (AuthenticationManager.User.IsInRole("admin"))
            {
                service.DeletePin(id);

                return RedirectToAction("ManagedPin");
            }
            return View("Index");
        }

        [Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            UserForIdentity userForIdentity = new UserForIdentity
            {
                Email = AuthenticationManager.User.Identity.Name,
                Name = AuthenticationManager.User.Identity.Name,
                Role = "user"

            };
            await UserService.DeleteUserAsync(userForIdentity);
            service.DeleteUser(AuthenticationManager.User.Identity.Name);
            AuthenticationManager.SignOut();
            return View("Index");
        }

        [Authorize]
        public async Task<ActionResult> DeleteUsers(string userName)
        {
            if (AuthenticationManager.User.IsInRole("admin") && userName!=null)
            {
                UserForIdentity userForIdentity = new UserForIdentity
                {
                    Email = userName,
                    Name = userName,
                    Role = "user"

                };
                await UserService.DeleteUserAsync(userForIdentity);
                service.DeleteUser(userName);
            }
            return RedirectToAction("ManagedUser");
        }

        [Authorize]
        public ActionResult ManagedUser()
        {
            if (AuthenticationManager.User.IsInRole("admin"))
            {
                List<UserDTO> list = new List<UserDTO>(service.UserGetAll());
                Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserRating>());

                var user =
                    Mapper.Map<IEnumerable<UserDTO>, List<UserRating>>(list);

                return View(user);
            }

            return View("Index");
        }

        [Authorize]
        public async Task<ActionResult> ChangePassword(string OldPassword , string NewPassword)
        {
            UserForIdentity userForIdentity = new UserForIdentity { Email = AuthenticationManager.User.Identity.Name, Password = OldPassword };
            ClaimsIdentity claim = await UserService.Authenticate(userForIdentity);
            if (claim == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
            }
            else
            {
                await UserService.ChangePasswordAsync(userForIdentity, NewPassword);
            }

            return RedirectToAction("Profiles");
        }

    }
}