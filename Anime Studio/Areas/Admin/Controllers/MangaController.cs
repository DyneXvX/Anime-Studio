using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anime_Studio.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class MangaController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public MangaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Manga manga = new Manga();

            if (id == null)
            {
                //this is a new manga
                return View(manga);
            }
            //this is for edit request
            manga = _unitOfWork.Manga.Get(id.GetValueOrDefault());

            if (manga == null)
            {
                return NotFound();
            }

            return View(manga);

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Manga manga)
        {
            if (ModelState.IsValid)
            {

                if (manga.Id == 0)
                {
                    _unitOfWork.Manga.Add(manga);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.Manga.Update(manga);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }

            return View(manga);
        }




        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Manga.GetAll();
            return Json(new {data = allObj});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Manga.Get(id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _unitOfWork.Manga.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
