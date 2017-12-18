using AutoMapper;
using ImagePinned.BLL.Interface;
using ListShop.BLL.EntitesDTO;
using ListShop.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ListShop.Web.Controllers
{
    public class ImageController : Controller
    {
        IService service;

        public ImageController(IService service)
        {

            this.service = service;

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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase uploadImage, ImageView imageModel)
        {
            
            if(ModelState.IsValid && uploadImage!=null)
            {
                byte[] imageData = null;
               
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                imageModel.Resurse = imageData;

                Mapper.Initialize(cfg => cfg.CreateMap<ImageView, ImageDTO>().ForMember("Id_pin",opt=>opt.MapFrom(c=>1)).ForMember("Id_Ownd",opt=>opt.MapFrom(c=>1)));
                var imageDTO =
                   Mapper.Map<ImageView, ImageDTO>(imageModel);
                service.CreateImage(imageDTO,imageModel.Pin,"Test");
            }
            return View("Index");
        }

        public PartialViewResult Likes(int Id)
        {
            service.Like("Test", Id);

            ImageDTOView imageDTOView = service.GetImage(Id);
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
              Mapper.Map<ImageDTOView, ImageModel>(imageDTOView);

            return PartialView(images);
        }

        public ActionResult Liked()
        {
            

            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetLike("Test"));
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);

            return View(images);

        }
    }
}