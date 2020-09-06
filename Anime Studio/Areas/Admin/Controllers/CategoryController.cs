using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anime_Studio.DataAccess.Data;
using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.Models;
using Anime_Studio.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anime_Studio.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = Sd.RoleAdmin)]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public CategoryController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();

            if (id == null)
            {
                //this is a new category
                return View(category);
            }
            //this is for edit request
            //category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            category = _db.Categories.FirstOrDefault(s => s.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {

                if (category.Id == 0)
                {
                    _unitOfWork.Category.Add(category);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.Category.Update(category);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }

            return View(category);
        }




        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Category.GetAll();
            return Json(new {data = allObj});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
