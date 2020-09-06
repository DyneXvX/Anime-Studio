using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anime_Studio.DataAccess.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUserRepository(ApplicationDbContext db, IUnitOfWork unitOfWork) : base(db)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

    }
}
