using AutoMapper;
using ImagePinned.BLL.Interface;
using ImagePinned.BLL.EntitesDTO;
using ImagePinned.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ListShop.Web.Validation;

namespace ImagePinned.Web.Controllers
{
    [ServerException]
    public class ImageController : Controller
    {

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
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Taped()
        {
            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
            images.Reverse();
            return View(images);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase uploadImage, ImageView imageModel)
        {

            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                imageModel.Resurse = imageData;
                try
                {
                    System.Drawing.Image x = (System.Drawing.Bitmap)((new System.Drawing.ImageConverter()).ConvertFrom(imageData));
                }
                catch (ArgumentException ex)
                {
                    return View();
                }
                Mapper.Initialize(cfg => cfg.CreateMap<ImageView, ImageDTO>().ForMember("Id_pin", opt => opt.MapFrom(c => 1)).ForMember("Id_Ownd", opt => opt.MapFrom(c => 1)));
                var imageDTO =
                   Mapper.Map<ImageView, ImageDTO>(imageModel);
                service.CreateImage(imageDTO, imageModel.Pin, AuthenticationManager.User.Identity.Name);
            }
            else
            {
                return View();
            }

            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
            images.Reverse();
            return View("Taped", images);

        }

        [Authorize]

        public PartialViewResult Like(int Id)
        {

            ImageDTOView imageDTOView = service.GetImage(Id);
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
              Mapper.Map<ImageDTOView, ImageModel>(imageDTOView);

            if (service.isLike(AuthenticationManager.User.Identity.Name, images.Id))
            {
                images.Pin = "true";
            }
            else
            {
                images.Pin = "false";
            }

            return PartialView("Likes", images);
        }

        [HttpGet]
        public PartialViewResult Likes(int Id)
        {
            service.Like(AuthenticationManager.User.Identity.Name, Id);

            ImageDTOView imageDTOView = service.GetImage(Id);
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
              Mapper.Map<ImageDTOView, ImageModel>(imageDTOView);
            if (service.isLike(AuthenticationManager.User.Identity.Name, images.Id))
            {
                images.Pin = "true";
            }
            else
            {
                images.Pin = "false";
            }

            return PartialView(images);
        }

        [Authorize]
        public ActionResult Liked()
        {


            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetLike(AuthenticationManager.User.Identity.Name));
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);

            return View(images);

        }

        public PartialViewResult Category(string Pin, string Own)
        {
            if (Pin == null)
            {
                List<ImageDTOView> list = new List<ImageDTOView>(service.GetImage(Own));
                Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
                var images =
                   Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
                images.Reverse();

                return PartialView(images);
            }
            else
            {
                List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll(Pin));
                Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
                var images =
                   Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
                images.Reverse();

                return PartialView(images);
            }

        }

        [Authorize]
        public PartialViewResult ShowPrefer()
        {
            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetPrefer(AuthenticationManager.User.Identity.Name));
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
            images.Reverse();

            return PartialView("Category", images);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreatePin(string Pin)
        {
            service.CreatePin(Pin);
            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
            images.Reverse();
            return View("Taped", images);
        }

    }
}