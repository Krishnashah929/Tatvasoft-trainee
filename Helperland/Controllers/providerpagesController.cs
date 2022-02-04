//using Helperland.Data;
//using Helperland.Models;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Helperland.Controllers
//{
//    public class providerpagesController : Controller
//    {
//        private readonly HelperlandContext _helperlandContext;


//        public providerpagesController(HelperlandContext helperlandContext)
//        {
//            _helperlandContext = helperlandContext;
//        }
//        [HttpGet]
//        public IActionResult service_provider()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult service_provider(User register)
//        {
//            if (ModelState.IsValid)
//            {
//                using (HelperlandContext objHelperlandContext = new HelperlandContext())
//                {
//                    register.CreatedDate = DateTime.Now;
//                    register.ModifiedDate = DateTime.Now;
//                    objHelperlandContext.Users.Add(register);
//                    objHelperlandContext.SaveChanges();
//                    // Int64 id = objEmployee.EmployeeID;
//                    ModelState.Clear();
//                }
//                // return View(objEmployee);
//            }
//            return View("service_provider");
//        }
//    }
//}
