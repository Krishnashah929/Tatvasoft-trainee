using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    public class publicpagesController : Controller
    {
        public IActionResult FAQ()
        {
            return View();
        }
        public IActionResult CONTACT()
        {
            return View();
        }
        public IActionResult ABOUT()
        {
            return View();
        }
        public IActionResult PRICES()
        {
            return View();
        }
        public IActionResult CREATE_ACCOUNT()
        {
            return View();
        }
    }
}
