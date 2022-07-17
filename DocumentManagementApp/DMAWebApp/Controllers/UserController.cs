using DataAccess;
using DataAccess.DBModels;
using DMAService;
using DMAWebApp.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMAWebApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        private readonly UserClient _client;
        public UserController(DMSDatabaseContext _context, IConfiguration _config)
        {
            _userService = new UserService(_context, _config);
            _client = new UserClient();
        }

        /*
         * GET LIST OF USERS
         */
        public async Task<IActionResult> Index()
        {
            var users = await _client.GetUsersAsync();
            return View(users);
        }

        /*
         * NEW USER CREATE FORM
         */
        public IActionResult Create()
        {
            return View();
        }

        /*
         * CREATE NEW USER
         */
        [HttpPost]
        public IActionResult Create(UserCreateModel user)
        {
            var status = _userService.Create(user);
            if (status)
            {
                ViewBag.success = "Created successfully";
            }
            else
            {
                ViewBag.error = "Error Occurred";
            }
            return View();
        }

        /*
         * DELETE USER BY ID
         */
        public IActionResult Delete(int id)
        {
            var status = _userService.Delete(id);
            if (status)
            {
                ViewBag.success = "Deleted successfully";
            }
            else
            {
                ViewBag.error = "Error Occurred";
            }
            return RedirectToAction("Index");
        }
    }
}
