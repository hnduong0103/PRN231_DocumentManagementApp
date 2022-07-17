using DataAccess;
using DataAccess.DBModels;
using DMAService;
using DMAWebApp.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMAWebApp.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ProjectController : Controller
    {
        private ProjectService _projectService;

        private ProjectClient _client;
        public ProjectController(DMSDatabaseContext context)
        {
            _projectService = new ProjectService(context);
            _client = new ProjectClient();
        }

        /*
         * PROJECT LIST
         */
        public async Task<IActionResult> Index()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var categories = await _client.GetProjectAsync(email);
            return View(categories);
        }

        /*
         * SHOW PROJECT CREATE FORM
         */
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /*
         * NEW PROJECT CREATE
         */
        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateModel project)
        {
            if (!ModelState.IsValid) return View();

            var email = HttpContext.Session.GetString("UserEmail");

            var status = await _client.CreateProjectAsync(project.ProjectName, email);
            dynamic res = JsonConvert.DeserializeObject(status);
            if (res.error != null)
            {
                ViewBag.error = null;
                ViewBag.success = "Created success";
            }
            else
            {
                ViewBag.success = null;
                ViewBag.error = "Error Occurred";
            }
            return View();
        }

        /*
         * PROJECT DELETE BY ID
         */

        public IActionResult Delete(int id)
        {
            var status = _projectService.DeleteProject(id);
            if (status)
            {
                TempData["success"] = "Deleted successfully";
            }
            else
            {
                TempData["Error"] = "Error Occurred";
            }
            return RedirectToAction("Index");
        }
    }
}
