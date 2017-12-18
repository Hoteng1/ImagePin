using ImagePinned.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImagePinned.DAL.Interfaces;
using AutoMapper;
using ListShop.BLL.EntitesDTO;
using ListShop.DAL.Entities;
using System.Security.Cryptography;

namespace ImagePinned.BLL.Provider
{
    public class Provider : IService
    {
        IUnitOfWork DataBase { get; set; }

        public Provider(IUnitOfWork unitOfWork)
        {
            DataBase = unitOfWork;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }

        public UserDTO Login(string login, string password)
        {
            return new UserDTO();
        }

        public void CreateImage(ImageDTO image,string teg , string own_name)
        {

            var id_ping = from c in DataBase.Pins.getAll() where c.Tititle == teg select c.Id;

            int id_pin = 0;
            foreach(int i in id_ping)
            {
                id_pin = i;
            }

            if(id_pin==0)
            {
                PinDTO pinDTO = new PinDTO();
                pinDTO.Tititle = teg;

                Mapper.Initialize(cfg => cfg.CreateMap<PinDTO, Pin>());
                var pin =
                   Mapper.Map<PinDTO, Pin>(pinDTO);
                this.DataBase.Pins.Create(pin);
                var pins = from c in DataBase.Pins.getAll() where c.Tititle == teg select c.Id;

                foreach (int id in pins)
                {
                    image.Id_pin = id;
                }
            }
            else
            {
                image.Id_pin = id_pin;  
            }
            

          
            image.Id_Ownd = getUserByName(own_name);
            Mapper.Initialize(cfg => cfg.CreateMap<ImageDTO, Image>());
            var imageDTO =
               Mapper.Map<ImageDTO, Image>(image);
            this.DataBase.Images.Create(imageDTO);


        }

        public void CreateUser(UserDTO userDTO)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, User>().ForMember("Password",org=>org.MapFrom(c=>Hash(c.Password))));
            var user =
               Mapper.Map<UserDTO, User>(userDTO);
            DataBase.Users.Create(user);
        }

        private string Hash(string password)
        {
            byte[] hash = Encoding.ASCII.GetBytes(password);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
            {
                result += b.ToString("x2");
            }
            return result;
        }

        public IEnumerable<ImageDTOView> ImageGetAll()
        {
            List<Image> list = new List<Image>(DataBase.Images.getAll());
            Mapper.Initialize(cfg => cfg.CreateMap<Image, ImageDTOView>().ForMember("Own_Name", org=>org.MapFrom(c=>getUserNameById(c.Id_Ownd))).ForMember("" +
                "Pin", ops => ops.MapFrom(c => getPinById(c.Id_pin))));
            var images =
               Mapper.Map<IEnumerable<Image>, List<ImageDTOView>> (list);

            return images;
        }

        private string getUserNameById(int id)
        {
            
            var users = from c in DataBase.Users.getAll() where c.Id==id select c.Login;
            string result = "anonym";
            foreach (string name in users)
            {
                result = name;
            }
            return result;
        }
        
        private string getPinById(int id)
        {
            var pins = from c in DataBase.Pins.getAll() where c.Id == id select c.Tititle;
            string result = "NoKnowlge";
            foreach (string name in pins)
            {
                result = name;
            }

            return result;
        }
        private int getUserByName(string name)
        {

            var users = from c in DataBase.Users.getAll() where c.Login == name select c.Id;
            int id_user=0;
            foreach (int id in users)
            {
                id_user = id;
            }

            return id_user;
        }

        public int Like(string login, int id_image)
        {
            int id_user = getUserByName(login);
            List<Like> list = new List<ListShop.DAL.Entities.Like>(DataBase.Likes.getAll());
            var users = from c in list where c.Id_Image == id_image select c;
            foreach(Like likes in users)
            {
                if(id_user==likes.Id_user)
                {
                    DataBase.Likes.Delete(likes.Id);

                    return Dislike(likes.Id_Image);
                }

            }

            LikeDTO likeDTO = new LikeDTO { Id_Image = id_image, Id_user = getUserByName(login) };
            Mapper.Initialize(cfg => cfg.CreateMap<LikeDTO, Like>());
            var like =
               Mapper.Map<LikeDTO, Like>(likeDTO);
            DataBase.Likes.Create(like);

            return Liked(like.Id_Image);

        }

        public int Dislike(int id_image)
        {
            Image image= DataBase.Images.getById(id_image);
            image.CountLike -= 1;
            DataBase.Images.Update(image);
            List<User> list = new List<User>(DataBase.Users.getAll());
            var users = from c in list where c.Login == getUserNameById(image.Id_Ownd) select c;
            User user = null;
            foreach (User select in users)
            {
                user = select;
            }
            if (user != null)
            {
                user.Count_like += 1;
                DataBase.Users.Update(user);
            }

            return image.CountLike;
        }

        public int Liked(int id_image)
        {
            Image image = DataBase.Images.getById(id_image);
            image.CountLike += 1;
            DataBase.Images.Update(image);
            List<User> list = new List<User>(DataBase.Users.getAll());
            var users = from c in list where c.Login == getUserNameById(image.Id_Ownd) select c;
            User user=null;
            foreach(User select in users)
            {
                user = select;
            }
            if(user!=null)
            {
                user.Count_like += 1;
                DataBase.Users.Update(user);
            }
            
            return image.CountLike;
        }

        public ImageDTOView GetImage(int id)
        {
            Image image = DataBase.Images.getById(id);
            Mapper.Initialize(cfg => cfg.CreateMap<Image, ImageDTOView>().ForMember("Own_Name", org => org.MapFrom(c => getUserNameById(c.Id_Ownd))).ForMember("" +
               "Pin", ops => ops.MapFrom(c => getPinById(c.Id_pin))));
            var imageDTO =
               Mapper.Map<Image, ImageDTOView>(image);

            return imageDTO;
        }

        public IEnumerable<ImageDTOView> ImageGetLike(string user_name)
        {
            int id_user = getUserByName(user_name);
            List<Like> likes =new List<ListShop.DAL.Entities.Like>(DataBase.Likes.getAll());
            var images = from c in likes where c.Id_user == id_user select c.Id_Image;
            List<Image> imageLikes = new List<Image>();
            foreach(int i in images)
            {
                imageLikes.Add(DataBase.Images.getById(i));
            }

            Mapper.Initialize(cfg => cfg.CreateMap<Image, ImageDTOView>().ForMember("Own_Name", org => org.MapFrom(c => getUserNameById(c.Id_Ownd))).ForMember("" +
               "Pin", ops => ops.MapFrom(c => getPinById(c.Id_pin))));
            var image =
               Mapper.Map<IEnumerable<Image>, List<ImageDTOView>>(imageLikes);

            return image;
        }
    }
}
