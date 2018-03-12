using ImagePinned.DAL.Interfaces;
using ImagePinned.DAL.Entities;
using ImagePinned.DAL.Indentity.EF;
using ImagePinned.DAL.Indentity.Entietis;
using ImagePinned.DAL.Indentity.Indenties;
using ImagePinned.DAL.Indentity.Intefaces;
using ImagePinned.DAL.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using ListShop.DAL.Validation;
using System.Data.SqlClient;

namespace ImagePinned.DAL.Indentity.Repository
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
            
                

        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }

        private UserRepository userRepository;

        private PinRepository pinRepository;

        private ImageRepository imageRepository;

        private PreferRepository preferRepository;

        private LikeRepository likeRepository;




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

        




        public void Save()
        {
            db.SaveChanges();
        }
    }
}
