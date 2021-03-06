using DataAccess.DBModels;
using DMAService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ProjectController(DMSDatabaseContext context)
        {
            _projectService = new ProjectService(context);
        }

        /*
         * PROJECT LIST
         */
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var categories = _projectService.GetAll(email);
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
        public IActionResult Create(Project project)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var status = _projectService.CreateProject(project, email);
            if (status)
            {
                ViewBag.success = "Created successfully";
            }
            else
            {
                ViewBag.error = "Something was wrong.";
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
