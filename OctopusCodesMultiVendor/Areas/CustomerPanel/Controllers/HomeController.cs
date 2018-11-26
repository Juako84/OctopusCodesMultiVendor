using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Areas.CustomerPanel.Controllers
{
    [Area("customerpanel")]
    [Route("customerpanel/login")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}