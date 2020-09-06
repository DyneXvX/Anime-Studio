using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Anime_Studio.DataAccess.Data;
using Anime_Studio.DataAccess.Data.Repository;
using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.Models;
using Anime_Studio.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Anime_Studio.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = Sd.RoleAdmin)]
    public class UserController : Controller
    {

        private readonly IUnitOfWork _unitOfWork; 
        private readonly ApplicationDbContext _db;
       

        public UserController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();

        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            
            var userList = _db.ApplicationUsers.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new {data = userList});
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while locking/unlocking"});
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();
            return Json(new {success = true, message = "Operation Successful."});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var objFromDb = _unitOfWork.Manga.Get(id);
            var objFromDb1 = _db.ApplicationUsers.FirstOrDefault();
            if (objFromDb1 == null)
            {
                return Json(new { success = false, message = "Error while deleting" }); }


            _db.ApplicationUsers.Remove(objFromDb1);
            //_unitOfWork.Manga.Remove(objFromDb);
            _db.SaveChanges();
            //_unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
