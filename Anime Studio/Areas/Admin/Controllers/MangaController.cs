using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.Models;
using Anime_Studio.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Anime_Studio.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class MangaController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment; // for the images required for the cover.

        public MangaController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            MangaVM mangaVM = new MangaVM()
            {
                Manga = new Manga(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is a new manga
                return View(mangaVM);
            }
            //this is for edit request
            mangaVM.Manga = _unitOfWork.Manga.Get(id.GetValueOrDefault());

            if (mangaVM.Manga == null)
            {
                return NotFound();
            }

            return View(mangaVM);

            
        }

        /*[HttpPost]
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
        }*/




        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Manga.GetAll(includeProperties:"Category");
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
