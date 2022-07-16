using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.DBModels;
using DMAAPI.Models;
using DMAService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DMAAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly DocumentService _documentService;
        private ProjectService _projectService;

        public DocumentController(
            IHostingEnvironment appEnvironment,
            DMSDatabaseContext context)
        {
            _appEnvironment = appEnvironment;
            _documentService = new DocumentService(context);
            _projectService = new ProjectService(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string str, int page = 1)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var documentList = await _documentService.GetList(email, str);
            int pageSize = 10;
            return new JsonResult(await PaginatedList<DocumentViewModel>.CreateAsync(documentList.AsNoTracking(), page, pageSize));
        }

        /*
         * SHOW DOCUMENT UPLOAD FORM
         */
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            return new JsonResult(email);
        }

        /*
         * UPLOAD NEW DOCUMENT
         */
        [HttpPost]
        [Route("create")]
        public IActionResult Create(IFormFile file, Document document)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var projects = _projectService.GetAll(email);
            string pathRoot = _appEnvironment.WebRootPath;
            var documentUploadResponse = _documentService.Upload(file, pathRoot, document, email);
            string error = null, success = null;
            if (documentUploadResponse.ContainsKey("error"))
            {
                error = documentUploadResponse["error"];
            }
            else
            {
                success = documentUploadResponse["success"];
            }

            return new JsonResult(new { projects = projects, error = error, success = success });
        }

        /*
         * DOWNLOAD DOCUMENT
         */
        [HttpGet]
        [Route("download/{id}")]
        public async Task<ActionResult> DownloadAsync(int id)
        {
            int documentId = (int) id;
            int userId = (int) HttpContext.Session.GetInt32("UserId");
            var status = _documentService.DocumentPermissionRule(userId, documentId);
            if (status)
            {
                string filePath = _documentService.GetPath(userId, documentId);
                string fileName = _documentService.GetName(userId, documentId);
                return new JsonResult(new {data = await this.ReturnDocumentFileAsync(filePath, fileName) });
            }
            else
            {
                return new JsonResult(new { error = "Document permission denied" });
            }
        }

        /*
         * RETURN FILE
         */
        [NonAction]
        public async Task<FileResult> ReturnDocumentFileAsync(string filePath, string fileName)
        {
            var path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\Documents\\", fileName
                           );

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        /*
         * GET CONTENT TYPES
         */
        [NonAction]
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        /*
         * GET MIME TYPES
         */
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
