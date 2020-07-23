using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anime_Studio.DataAccess.Data;
using Anime_Studio.DataAccess.Data.Repository.IRepository;
using Anime_Studio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anime_Studio.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {

        //private readonly IUnitOfWork _unitOfWork; Removing UnitOfWork for this example. 
        private readonly ApplicationDbContext _db;             

        public UserController(ApplicationDbContext db)
        {
           
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


        #endregion
    }
}
