using DMAWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DMAWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /*
         * HOMEPAGE
         */
        public IActionResult Index()
        {
            return View();
        }

        /*
         * ERROR
         */
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
