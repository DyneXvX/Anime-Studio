using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anime_Studio.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category{ get; }
        IMangaRepository Manga { get; }
        ISP_Call SP_Call{ get; }

        IApplicationUserRepository ApplicationUser{ get; }

        void Save();
    }
}
