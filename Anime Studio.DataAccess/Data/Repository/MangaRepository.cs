using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anime_Studio.DataAccess.Data.Repository
{
    public class MangaRepository : Repository<Manga>, IMangaRepository
    {
        private readonly ApplicationDbContext _db;

        public MangaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Manga manga)
        {
            var objFromDb = _db.Mangas.FirstOrDefault(s => s.Id == manga.Id);
            
            if (objFromDb != null)
            {
                objFromDb.Title = manga.Title;
                objFromDb.Author = manga.Author;
                objFromDb.Artist = manga.Artist;
                objFromDb.ISBN = manga.ISBN;
                objFromDb.PublishingCompany = manga.PublishingCompany;
                objFromDb.VolumeNumber = manga.VolumeNumber;
                objFromDb.Price = manga.Price;
                objFromDb.Cover = manga.Cover;
            }
        }


    }
}
