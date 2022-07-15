using DataAccess;
using DataAccess.DBModels;
using DMAAPI.Models;
using DMAService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DMAAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(DMSDatabaseContext _context, IConfiguration _config)
        {
            _userService = new UserService(_context, _config);
        }

        /*
         * GET LIST OF USERS
         */
        public async Task<IActionResult> IndexAsync(string searchStr, int page = 1)
        {
            var users = await _userService.GetAll(searchStr);
            int pageSize = 10;
            return new JsonResult(await PaginatedList<UserViewModel>.CreateAsync(users.AsNoTracking(), page, pageSize));
        }

        /*
         * NEW USER CREATE FORM
         */
        //public IActionResult Create()
        //{
        //    return View();
        //}

        /*
         * CREATE NEW USER
         */
        [HttpPost]
        public IActionResult Create(User user)
        {
            var status = _userService.Create(user);
            string success = null;
            string error = null;
            if (status)
            {
                success = "Created successfully";
            }
            else
            {
                error = "Error Occurred";
            }
            return new JsonResult(new { status = status, error = error, success = success });
        }

        /*
         * DELETE USER BY ID
         */
        public IActionResult Delete(int id)
        {
            var status = _userService.Delete(id);
            string success = null;
            string error = null;
            if (status)
            {
                success = "Deleted successfully";
            }
            else
            {
                error = "Error Occurred";
            }
            return new JsonResult(new { status = status, error = error, success = success });
        }
    }
}
