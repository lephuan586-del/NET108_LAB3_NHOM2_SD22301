using Microsoft.AspNetCore.Mvc;
using NET108_LAB3_NHOM2_SD22301.Models;
using System.Diagnostics;

namespace NET108_LAB3_NHOM2_SD22301.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
