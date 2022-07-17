using DataAccess;
using DataAccess.DBModels;
using DMAService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
        [HttpGet]
        public IActionResult Index()
        {
            var users = _userService.GetAll();
            return new JsonResult(users);
        }


        /*
         * CREATE NEW USER
         */
        [HttpPost]
        public IActionResult Create(UserCreateModel user)
        {
            string success = null;
            string error = null;
            var status = _userService.Create(user);
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
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            string success = null;
            string error = null;
            var status = _userService.Delete(id);
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
