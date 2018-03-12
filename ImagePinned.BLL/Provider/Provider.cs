using ImagePinned.BLL.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ImagePinned.BLL.EntitesDTO;
using ImagePinned.DAL.Entities;
using System.Security.Cryptography;
using ImagePinned.DAL.Indentity.Intefaces;
using System.Data.SqlClient;
using ListShop.DAL.Validation;
using ListShop.BLL.Validation;

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

        

        public void CreateImage(ImageDTO image,string teg , string own_name)
        {

            var id_ping = from c in DataBase.Pins.getAll() where c.Title == teg select c.Id;

            int id_pin = 0;
            foreach(int i in id_ping)
            {
                id_pin = i;
            }

            if(id_pin==0)
            {
                PinDTO pinDTO = new PinDTO();
                pinDTO.Title = teg;

                Mapper.Initialize(cfg => cfg.CreateMap<PinDTO, Pin>());
                var pin =
                   Mapper.Map<PinDTO, Pin>(pinDTO);
                this.DataBase.Pins.Create(pin);
                var pins = from c in DataBase.Pins.getAll() where c.Title == teg select c.Id;

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
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, User>());
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
            try
            {
                List<Image> list = new List<Image>(DataBase.Images.getAll());
                Mapper.Initialize(cfg => cfg.CreateMap<Image, ImageDTOView>().ForMember("Own_Name", org => org.MapFrom(c => getUserNameById(c.Id_Ownd))).ForMember("" +
                    "Pin", ops => ops.MapFrom(c => getPinById(c.Id_pin))));
                var images =
                   Mapper.Map<IEnumerable<Image>, List<ImageDTOView>>(list);

                return images;
            }
            catch(DALException ex)
            {
                throw new BLLException(ex.Message);
            }
           
        }

        public IEnumerable<ImageDTOView> ImageGetAll(string Pin)
        {
            List<Image> list = new List<Image>(DataBase.Images.getAll());
           
            Mapper.Initialize(cfg => cfg.CreateMap<Image, ImageDTOView>().ForMember("Own_Name", org => org.MapFrom(c => getUserNameById(c.Id_Ownd))).ForMember("" +
                "Pin", ops => ops.MapFrom(c => getPinById(c.Id_pin))));
            var images =
               Mapper.Map<IEnumerable<Image>, List<ImageDTOView>>(list);

            var sortlist = from c in images where c.Pin == Pin select c;

            return sortlist;
        }

        public IEnumerable<UserDTO> UserGetAll()
        {
            List<User> list = new List<User>(DataBase.Users.getAll());

            Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());
            var users =
               Mapper.Map<IEnumerable<User>, List<UserDTO>>(list);

            return users;
        }

        private string getUserNameById(int id)
        {
            try
            {
                var users = from c in DataBase.Users.getAll() where c.Id == id select c.Login;
                string result = "anonym";
                foreach (string name in users)
                {
                    result = name;
                }
                return result;
            }
            catch(DALException ex)
            {
                throw new BLLException(ex.Message);
            }
          
        }
        
        private string getPinById(int id)
        {
            var pins = from c in DataBase.Pins.getAll() where c.Id == id select c.Title;
            string result = "NoKnowlge";
            foreach (string name in pins)
            {
                result = name;
            }

            return result;
        }

        private int getPinByName(string Name)
        {
            var pins = from c in DataBase.Pins.getAll() where c.Title == Name select c.Id;
            int pin = 0;
            foreach (int item in pins)
            {
                pin = item;
            }

            return pin;
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
            List<Like> list = new List<ImagePinned.DAL.Entities.Like>(DataBase.Likes.getAll());
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
                user.Count_like -= 1;
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
            List<Like> likes =new List<ImagePinned.DAL.Entities.Like>(DataBase.Likes.getAll());
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

        public UserDTO GetUser(string name)
        {
            int id = getUserByName(name);
            User user = DataBase.Users.getById(id);

            Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());
            var userDTO = Mapper.Map<User, UserDTO>(user);

            return userDTO;
        }

        public IEnumerable<PinDTO> GetPinAll()
        {
            List<Pin> list =new List<Pin>(DataBase.Pins.getAll());

            Mapper.Initialize(cfg => cfg.CreateMap<Pin, PinDTO>());
            var pins =
                Mapper.Map<IEnumerable<Pin>, List<PinDTO>>(list);

            return pins;
        }


        public void AddPrefer(string title,string user)
        {
            
            Prefer prefer = new Prefer
            {
                Id_pin = getPinByName(title),
                Id_user = getUserByName(user)

            };
            var id_prefer = from c in DataBase.Prefers.getAll() where c.Id_user == prefer.Id_user && c.Id_pin == prefer.Id_pin select c.Id;

            int id_pin = 0;
            foreach (int i in id_prefer)
            {
                id_pin = i;
            }

               if(id_pin==0)
            DataBase.Prefers.Create(prefer);
        }

        public IEnumerable<PinDTO> GetPinAll(string user)
        {
           

            List<Pin> list = new List<Pin>(DataBase.Pins.getAll());

            var chooseList = from c in list where CheckPinAtPrefer(c, user) select c;

            Mapper.Initialize(cfg => cfg.CreateMap<Pin, PinDTO>());
            var pins =
                Mapper.Map<IEnumerable<Pin>, List<PinDTO>>(chooseList);

            return pins;
        }

        private bool CheckPinAtPrefer(Pin pin, string user)
        {
            List<Prefer> listPrefer = new List<Prefer>(DataBase.Prefers.getAll());
            foreach(Prefer item in listPrefer)
            {
                if(item.Id_pin==pin.Id && item.Id_user==getUserByName(user))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<PinDTO> GetNotPinAll(string user)
        {
            List<Pin> list = new List<Pin>(DataBase.Pins.getAll());

            var chooseList = from c in list where !CheckPinAtPrefer(c, user) select c;

            Mapper.Initialize(cfg => cfg.CreateMap<Pin, PinDTO>());
            var pins =
                Mapper.Map<IEnumerable<Pin>, List<PinDTO>>(chooseList);

            return pins;
        }

        public void DeletePrefer(string pin,string user)
        {
            Prefer prefer = new Prefer
            {
                Id_pin = getPinByName(pin),
                Id_user = getUserByName(user)

            };
            var id_prefer = from c in DataBase.Prefers.getAll() where c.Id_user == prefer.Id_user && c.Id_pin == prefer.Id_pin select c.Id;
            
            int id_pin = 0;
            foreach (int i in id_prefer)
            {
                id_pin = i;
            }
            if (id_pin != 0)
                DataBase.Prefers.Delete(id_pin);
        }

        public IEnumerable<ImageDTOView> ImageGetPrefer(string User)
        {
            List<Image> list = new List<Image>(DataBase.Images.getAll());

            Mapper.Initialize(cfg => cfg.CreateMap<Image, ImageDTOView>().ForMember("Own_Name", org => org.MapFrom(c => getUserNameById(c.Id_Ownd))).ForMember("" +
                "Pin", ops => ops.MapFrom(c => getPinById(c.Id_pin))));
            var images =
           Mapper.Map<IEnumerable<Image>, List<ImageDTOView>>(list);
            
            List<Prefer> prefers = new List<Prefer>(DataBase.Prefers.getAll());
            int id_user = getUserByName(User);
            var pins = from c in prefers where c.Id_user == id_user select c.Id_pin;
            var sortlist = from c in images where pins.Contains(getPinByName(c.Pin)) select c;

            return sortlist;
        }

        

        public void Delete(int id)
        {
            if(DataBase.Images.getById(id)!=null)
            {
                List<Like> listLike = new List<ImagePinned.DAL.Entities.Like>(DataBase.Likes.getAll());
                var list = from c in listLike where c.Id_Image == id select c.Id;
                foreach (int idLike in list)
                {
                    DataBase.Likes.Delete(idLike);
                    Dislike(id);
                }

                DataBase.Images.Delete(id);
            }
            
        }

        public void Update(ImageDTOView imageDTO)
        {
            if(imageDTO!=null)
            {
                Image image;
                var id_ping = from c in DataBase.Pins.getAll() where c.Title == imageDTO.Pin select c.Id;

                int id_pin = 0;
                foreach (int i in id_ping)
                {
                    id_pin = i;
                }
                if (id_pin != 0)
                {
                    image = DataBase.Images.getById(imageDTO.Id);
                    image.Title = imageDTO.Title;
                    image.Id_pin = id_pin;

                }
                else
                {
                    PinDTO pinDTO = new PinDTO();
                    pinDTO.Title = imageDTO.Pin;

                    Mapper.Initialize(cfg => cfg.CreateMap<PinDTO, Pin>());
                    var pin =
                       Mapper.Map<PinDTO, Pin>(pinDTO);
                    this.DataBase.Pins.Create(pin);
                    var pins = from c in DataBase.Pins.getAll() where c.Title == imageDTO.Pin select c.Id;

                    foreach (int id in pins)
                    {
                        id_pin = id;
                    }

                    image = DataBase.Images.getById(imageDTO.Id);
                    image.Title = image.Title;
                    image.Id_pin = id_pin;

                }
              

               DataBase.Images.Update(image);
            }
            
        }

        public bool isLike(string user, int id_image)
        {
            List<Like> list = new List<ImagePinned.DAL.Entities.Like>(DataBase.Likes.getAll());
            int id_user = getUserByName(user);
            var sortList = from i in list where i.Id_Image == id_image && i.Id_user == id_user select i.Id;

            int result = 0;

            foreach(int id in sortList)
            {
                result = id;
            }

            if(result!=0)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<ImageDTOView> GetImage(string userName)
        {
            List<Image> list = new List<Image>(DataBase.Images.getAll());
            Mapper.Initialize(cfg => cfg.CreateMap<Image, ImageDTOView>().ForMember("Own_Name", org => org.MapFrom(c => getUserNameById(c.Id_Ownd))).ForMember("" +
                "Pin", ops => ops.MapFrom(c => getPinById(c.Id_pin))));
            var images =
               Mapper.Map<IEnumerable<Image>, List<ImageDTOView>>(list);
            

            var listImage = from c in images where c.Own_Name == userName select c;

            return listImage;
        }

        public void CreatePin(string pin)
        {
            var id_ping = from c in DataBase.Pins.getAll() where c.Title == pin select c.Id;

            int id_pin = 0;
            foreach (int i in id_ping)
            {
                id_pin = i;
            }
            if (id_pin == 0)
            {
                PinDTO pinDTO = new PinDTO();
                pinDTO.Title = pin;

                Mapper.Initialize(cfg => cfg.CreateMap<PinDTO, Pin>());
                var pinD =
                   Mapper.Map<PinDTO, Pin>(pinDTO);
                this.DataBase.Pins.Create(pinD);
            }
        }

        public IEnumerable<PinDTOView> GetPins()
        {
            List<Pin> list = new List<Pin>(DataBase.Pins.getAll());
            List<Image> listImage = new List<Image>(DataBase.Images.getAll());
            List<int> listIdPin = new List<int>();
            foreach(var item in listImage)
            {
                listIdPin.Add(item.Id_pin);
            }
            var sortedList = from c in list where !listIdPin.Contains(c.Id) select c;

            Mapper.Initialize(cfg => cfg.CreateMap<Pin, PinDTOView>());
            var pins =
               Mapper.Map<IEnumerable<Pin>, List<PinDTOView>>(sortedList);

            return pins;

        }

        public void DeletePin(int id)
        {
            List<Image> listImage = new List<Image>(DataBase.Images.getAll());
            List<int> listIdPin = new List<int>();
            foreach (var item in listImage)
            {
                listIdPin.Add(item.Id_pin);
            }
            if(!listIdPin.Contains(id))
            {
                List<Prefer> list = new List<Prefer>(DataBase.Prefers.getAll());

                var listSort = from c in list where c.Id_pin == id select c.Id;

                foreach (int Id in listSort)
                {
                    DataBase.Prefers.Delete(Id);
                }

                DataBase.Pins.Delete(id);
            }

           
        }

        public void DeleteUser(string user)
        {
            int id_user = getUserByName(user);

            List<Prefer> list = new List<Prefer>(DataBase.Prefers.getAll());

            var PreferListId = from c in list where c.Id_user == id_user select c.Id;

            foreach(int id in PreferListId)
            {
                DataBase.Prefers.Delete(id);
            }

            List<Image> imageList = new List<Image>(DataBase.Images.getAll());

            var ImageListId = from c in imageList where c.Id_Ownd == id_user select c.Id;
            
            foreach(int id in ImageListId)
            {
                Dislike(id);
            }
            foreach(int id in ImageListId)
            {
                DataBase.Images.Delete(id);
            }

            DataBase.Users.Delete(id_user);
        }

       
    }
}
