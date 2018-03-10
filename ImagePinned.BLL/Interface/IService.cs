
using ImagePinned.BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.BLL.Interface
{
    public interface IService : IDisposable
    {
        void CreateImage(ImageDTO image,string teg , string own_name);
        void CreateUser(UserDTO userDTO);
        IEnumerable<ImageDTOView> ImageGetAll();
        IEnumerable<ImageDTOView> ImageGetAll(string Pin);
        ImageDTOView GetImage(int id);
        int Like(string login, int id_image);
        IEnumerable<ImageDTOView> ImageGetLike(string user_name); 
        void Dispose();
        void AddPrefer(string title, string user);
        IEnumerable<PinDTO> GetPinAll();
        IEnumerable<PinDTO> GetPinAll(string user);
        IEnumerable<PinDTO> GetNotPinAll(string user);
        void DeletePrefer(string pin,string user);
        UserDTO GetUser(string name);
        IEnumerable<UserDTO> UserGetAll();
        IEnumerable<ImageDTOView> ImageGetPrefer(string User);
        void Delete(int id);
        void Update(ImageDTOView imageDTO);
        bool isLike(string user, int id_image);
        IEnumerable<ImageDTOView> GetImage(string userName);
        void CreatePin(string pin);
        IEnumerable<PinDTOView> GetPins();
        void DeletePin(int id);
        void DeleteUser(string User);

    }
}
