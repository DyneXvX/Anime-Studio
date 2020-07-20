using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(MangaVM mangavm)
        {
            if (ModelState.IsValid)
            {

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\mangas");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (mangavm.Manga.Cover != null)
                    {
                        //this is to edit the cover and remove the old image.
                        var imagePath = Path.Combine(webRootPath, mangavm.Manga.Cover.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var filesStreams =
                        new FileStream(Path.Combine(uploads, filename + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }

                    mangavm.Manga.Cover = @"\images\mangas\" + filename + extenstion;
                }
                else
                {
                    //update if the cover is not changed.
                    if (mangavm.Manga.Id != 0)
                    {
                        Manga objFromDb = _unitOfWork.Manga.Get(mangavm.Manga.Id);
                        mangavm.Manga.Cover = objFromDb.Cover;
                    }
                }

                if (mangavm.Manga.Id == 0)
                {
                    _unitOfWork.Manga.Add(mangavm.Manga);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.Manga.Update(mangavm.Manga);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                mangavm.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }

            if (mangavm.Manga.Id != 0)
            {
                mangavm.Manga = _unitOfWork.Manga.Get(mangavm.Manga.Id);
            }
            return View(mangavm);
        }




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
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.Cover.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Manga.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
