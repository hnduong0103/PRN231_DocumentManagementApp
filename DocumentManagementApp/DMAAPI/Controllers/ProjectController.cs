using DataAccess;
using DataAccess.DBModels;
using DMAService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMAAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private ProjectService _projectService;
        public ProjectController(DMSDatabaseContext context)
        {
            _projectService = new ProjectService(context);
        }

        /*
         * PROJECT LIST
         */
        [HttpGet]
        public IActionResult Index(string email)
        {
            //var email = HttpContext.Session.GetString("UserEmail");
            var projects = _projectService.GetAll(email);
            return  new JsonResult(projects);
        }

        /*
         * NEW PROJECT CREATE
         */
        [HttpPost]
        public IActionResult Create(string email, [FromBody] ProjectCreateModel project)
        {
            //var email = HttpContext.Session.GetString("UserEmail");
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
        [HttpDelete]
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
