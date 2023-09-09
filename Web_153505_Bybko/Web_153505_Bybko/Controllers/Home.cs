using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153505_Bybko.Models;

namespace Web_153505_Bybko.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            ViewData["LabTitle"] = "Laboratory work 1-2";

            ViewData.Model = new HomeViewModel();

            return View();
        }
    }
}
