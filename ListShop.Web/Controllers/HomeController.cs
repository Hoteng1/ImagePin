using ImagePinned.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ListShop.Web.Models;
using Ninject;
using ListShop.BLL.EntitesDTO;

namespace ImagePinned.Web.Controllers
{
    public class HomeController : Controller
    {
        IService service;

        public HomeController(IService service)
        {
            
            this.service = service;
            
        }
        // GET: Home
        public ActionResult Index()
        {
            
            return View();
        }

        

        public PartialViewResult UserLogin()
        {
            
            return PartialView(null);
        }

        public ActionResult Rating()
        {
            List<ImageDTOView> list = new List<ImageDTOView>(service.ImageGetAll());
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTOView, ImageModel>());
            var images =
               Mapper.Map<IEnumerable<ImageDTOView>, List<ImageModel>>(list);
           
            return View(Sorted(images));
        }

        private IEnumerable<ImageModel> Sorted(IEnumerable<ImageModel> list)
        {
            List<ImageModel> result =new List<ImageModel>(list.OrderBy(s => s.CountLike));

            return result;

        }

        


    }
}