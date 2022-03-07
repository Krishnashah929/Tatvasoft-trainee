using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helperland.Data;
using Helperland.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Helperland.Controllers
{
    public class HomeController : Controller
    {
        private readonly HelperlandContext _helperlandContext;
        public HomeController(HelperlandContext helperlandContext)
        {
            _helperlandContext = helperlandContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User user)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                 
                var p = ObjHelperlandContext.Users.Where(c => c.Email == user.Email && c.Password == user.Password).ToList();
                ModelState.Clear();
                if (p.Count == 1)
                {
                    User userdetails = ObjHelperlandContext.Users.Where(c => c.Email == user.Email && c.Password == user.Password).FirstOrDefault();
                    var Name = userdetails.FirstName + " " + userdetails.LastName;
                    ViewBag.userType = user.UserTypeId;

                    HttpContext.Session.SetString("Userlogeddin", "true");
                    HttpContext.Session.SetString("Name", Name);
                    HttpContext.Session.SetInt32("userID", userdetails.UserId);

                    if (p.FirstOrDefault().UserTypeId == 1)
                    {
                        HttpContext.Session.SetString("UserTypeId", user.UserTypeId.ToString());
                        return RedirectToAction("Customer_Dashboard", "CustomerPages");
                    }
                     else if (p.FirstOrDefault().UserTypeId == 2)
                    {
                        HttpContext.Session.SetString("UserTypeId", user.UserTypeId.ToString());
                        return RedirectToAction("service_provider", "Home");
                    }
                }
                else 
                {
                    ViewBag.Message = "Your previous details are Invalid. " +  "Please enter valid details.";
                }
                //forgot password code
                String ResetCode = Guid.NewGuid().ToString();

                var uriBuilder = new UriBuilder
                {
                    Scheme = Request.Scheme,
                    Host = Request.Host.Host,
                    Port = Request.Host.Port ?? -1, //bydefault -1
                    Path = $"/Home/ForgotPassword/{ResetCode}"
                };
                var link = uriBuilder.Uri.AbsoluteUri;

                var getUser = (from s in ObjHelperlandContext.Users where s.Email == user.Email select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = ResetCode;
                    ObjHelperlandContext.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.FirstName + ", <br/> You recently requested to reset the password for your account. Click the link below to reset ." +
                     "<br/> <br/><a href='" + link + "'>" + link + "</a> <br/> <br/>" +
                    "If you did not request for reset password please ignore this mail.";

                    SendEmail(getUser.Email, body, subject);

                    TempData["SuccessMsg"] = "Your Success Message";
                    return RedirectToAction("Index", "Home");
                    //ViewBag.Message1 = "Reset password link has been sent to your email id";
                }
                //else
                //{
                //    TempData["SuccessMsg"] = "Your Success Message";
                //    return RedirectToAction("Index", "Home");
                //}
            }
            TempData["FailMsg"] = "Your Success Message";
            return View();
        }

        private void SendEmail(string emailaddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", emailaddress))
            {
                //MailAddress by = new MailAddress("kdshah929@gmail.com");
                mm.Subject = subject;
                mm.Body = body;
                //mm.From = by;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    EnableSsl = true
                };
                NetworkCredential NetworkCred = new NetworkCredential("krishnaa9121@gmail.com", "Kri$hn@91");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        public IActionResult ForgotPassword(String id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var user = ObjHelperlandContext.Users.Where(f => f.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ForgotPassword modal = new ForgotPassword();
                    modal.ResetCode = id;
                    return View(modal);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
                {
                    var user = ObjHelperlandContext.Users.Where(f => f.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.Password = model.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        ObjHelperlandContext.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message1 = message;
            return View(model);
        }
    
        [HttpGet]
        public IActionResult CREATE_ACCOUNT()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create_Account(User signup)
        {
            if(ModelState.IsValid)
            {
                if (_helperlandContext.Users.Where(s => s.Email == signup.Email).Count() == 0 && _helperlandContext.Users.Where(s => s.Mobile == signup.Mobile).Count() == 0)
                {
                    signup.UserTypeId = 1;
                    signup.CreatedDate = DateTime.Now;
                    signup.ModifiedDate = DateTime.Now;
                    signup.IsRegisteredUser = true;
                    signup.ModifiedBy = 123;

                    _helperlandContext.Users.Add(signup);
                    _helperlandContext.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "This Email and Mobile are already registerd.";
                }
            }
            return View();
        }
    
        [HttpGet]
        public IActionResult service_provider()
        {
            return View();
        }
        [HttpPost]
        public IActionResult service_provider(User register)
        {
            if (ModelState.IsValid)
            {
                if (_helperlandContext.Users.Where(r => r.Email == register.Email).Count() == 0 && _helperlandContext.Users.Where(r => r.Mobile == register.Mobile).Count() == 0)
                {
                    register.UserTypeId = 2;
                    register.CreatedDate = DateTime.Now;
                    register.ModifiedDate = DateTime.Now;
                    register.IsRegisteredUser = true;
                    register.ModifiedBy = 123;

                    _helperlandContext.Users.Add(register);
                    _helperlandContext.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "This Email and Mobile are already registerd.";
                }
            }
            return View("service_provider");
        }
       
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            TempData["LogOutMsg"] = "Logged out";
            return RedirectToAction("Index", "Home");
             
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
