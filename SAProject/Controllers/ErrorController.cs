using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SAProject.Controllers
{
    [Route("ErrorPage/{statuscode}")]
    public class ErrorController : Controller
    {
        public IActionResult Index(int statuscode)
        {
            switch (statuscode)
            {
                case 404:
                    ViewData["Error"] = "page not found";
                    break;
                case 500:
                    ViewData["Error"] = "Something went wrong";
                    break;
                default:
                    break;
            }
            return View("ErrorPage");
        }
    }
}
