using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace organiza_med_tcc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
