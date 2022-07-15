using DataAccess;
using DataAccess.DBModels;
using DMAAPI.Models;
using DMAService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DMAAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;
        public ProjectController(DMSDatabaseContext context)
        {
            _projectService = new ProjectService(context);
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(string searchStr, int page = 1)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var categories = _projectService.GetAll(email, searchStr);
            int pageSize = 10;
            return new JsonResult(await PaginatedList<ProjectViewModel>.CreateAsync(categories.AsNoTracking(), page, pageSize));
        }

        /*
         * SHOW PROJECT CREATE FORM
         */
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        /*
         * NEW PROJECT CREATE
         */
        [HttpPost]
        public IActionResult Create(Project project)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var status = _projectService.CreateProject(project, email);
            string error = null;
            string success = null;
            if (status)
            {
                success = "Created successfully";
            }
            else
            {
                error = "Something was wrong.";
            }
            return new JsonResult(new { status = status, error = error, success = success });
        }

        /*
         * PROJECT DELETE BY ID
         */

        public IActionResult Delete(int id)
        {
            var status = _projectService.DeleteProject(id);
            string error = null;
            string success = null;
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
