
using ListShop.BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.BLL.Interface
{
    public interface IService
    {
        UserDTO Login(string login, string password);
        void CreateImage(ImageDTO image,string teg , string own_name);
        void CreateUser(UserDTO userDTO);
        IEnumerable<ImageDTOView> ImageGetAll();
        ImageDTOView GetImage(int id);
        int Like(string login, int id_image);
        IEnumerable<ImageDTOView> ImageGetLike(string user_name); 
        void Dispose();
        
    }
}
