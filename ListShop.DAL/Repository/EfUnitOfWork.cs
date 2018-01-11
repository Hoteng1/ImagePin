using ImagePinned.DAL.Interfaces;
using ImagePinned.DAL.EF;
using ListShop.DAL.Repository;
using ListShop.DAL.Entities;

namespace ImagePinned.DAL.Repository
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private PinContext db;

        private UserRepository userRepository;

        private PinRepository pinRepository;

        private ImageRepository imageRepository;

        private PreferRepository preferRepository;

        private LikeRepository likeRepository;

        public EfUnitOfWork(string conectionString)
        {
            this.db = new PinContext(conectionString);
        }

     
        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public IRepository<Pin> Pins
        {
            get
            {
                if (pinRepository == null)
                    pinRepository = new PinRepository(db);
                return pinRepository;
            }
        }

        public IRepository<Prefer> Prefers
        {
            get
            {
                if (preferRepository == null)
                    preferRepository = new PreferRepository(db);
                return preferRepository;
            }
        }

        public IRepository<Image> Images
        {
            get
            {
                if (imageRepository == null)
                    imageRepository = new ImageRepository(db);
                return imageRepository;
            }
        }

        public IRepository<Like> Likes
        {
            get
            {
                if (likeRepository == null)
                    likeRepository = new LikeRepository(db);
                return likeRepository;
            }
        }



        public void Dispose()
        {
            db.Dispose();   
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
