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

        #region Index(GET)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Index(POST)
        [HttpPost]
        public IActionResult Index(User user)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {

                var p = ObjHelperlandContext.Users.Where(c => c.Email == user.Email && c.Password == user.Password).ToList();
                ModelState.Clear();
                if (p.Count == 1)
                {
                    User userdetails = ObjHelperlandContext.Users.Where(c => c.Email == user.Email && c.Password == user.Password && c.IsApproved == true).FirstOrDefault();
                    if(userdetails!= null)
                    {
                        var Name = userdetails.FirstName + " " + userdetails.LastName;
                        ViewBag.userType = user.UserTypeId;

                        HttpContext.Session.SetString("Userlogeddin", "true");

                        HttpContext.Session.SetString("Name", Name);
                        HttpContext.Session.SetInt32("userID", userdetails.UserId);

                        if (p.FirstOrDefault().UserTypeId == 1)
                        {
                            HttpContext.Session.SetString("UserTypeId", userdetails.UserTypeId.ToString());
                            return RedirectToAction("Customer_Dashboard", "CustomerPages");
                        }
                        else if (p.FirstOrDefault().UserTypeId == 2)
                        {

                            HttpContext.Session.SetString("UserTypeId", userdetails.UserTypeId.ToString());
                            return RedirectToAction("Provider_Dashboard", "ProviderPages");

                        }
                        else if (p.FirstOrDefault().UserTypeId == 3)
                        {
                            HttpContext.Session.SetString("UserTypeId", userdetails.UserTypeId.ToString());
                            return RedirectToAction("Admin_Index", "AdminPages");

                        }
                    }
                    else
                    {
                        TempData["NotApprove"] = "Message";
                    }
                }
                else
                {
                    TempData["ErrorMsg"] = "Your Error Message"; 
                }
            }
            return View();
        }
        #endregion

        #region Sendlink
        [HttpPost]
        public IActionResult Sendlink(User user)
        {
            var p = _helperlandContext.Users.Where(c => c.Email == user.Email && c.Password == user.Password).ToList();
            ModelState.Clear();
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

            var getUser = (from s in _helperlandContext.Users where s.Email == user.Email select s).FirstOrDefault();
            if (getUser != null)
            {
                getUser.ResetPasswordCode = ResetCode;
                _helperlandContext.SaveChanges();

                var subject = "Password Reset Request";
                var body = "Hi " + getUser.FirstName + ", <br/> You recently requested to reset the password for your account. Click the link below to reset ." +
                 "<br/> <br/><a href='" + link + "'>" + link + "</a> <br/> <br/>" +
                "If you did not request for reset password please ignore this mail.";

                SendEmail(getUser.Email, body, subject);

                TempData["SuccessMsg"] = "Your Success Message";
                return RedirectToAction("Index", "Home");
                //ViewBag.Message1 = "Reset password link has been sent to your email id";
            }
            else
            {
                TempData["FailMsg"] = "Your Success Message";
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region SendEmail
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
        #endregion

        #region ForgotPassword
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
        #endregion

        #region ForgotPassword(POST)
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
        #endregion

        #region CREATE_ACCOUNT(GET)
        [HttpGet]
        public IActionResult CREATE_ACCOUNT()
        {
            return View();
        }
        #endregion

        #region Create_Account(POST)
        [HttpPost]
        public IActionResult Create_Account(User signup)
        {
            if (ModelState.IsValid)
            {
                if (_helperlandContext.Users.Where(s => s.Email == signup.Email).Count() == 0 && _helperlandContext.Users.Where(s => s.Mobile == signup.Mobile).Count() == 0)
                {
                    signup.UserTypeId = 1;
                    signup.CreatedDate = DateTime.Now;
                    signup.ModifiedDate = DateTime.Now;
                    signup.IsRegisteredUser = true;
                    signup.ModifiedBy = 123;
                    signup.IsActive = true;
                    signup.IsApproved = true;
                    signup.Status = 1; // status 1 is true or active

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
        #endregion

        #region service_provider(GET)
        [HttpGet]
        public IActionResult service_provider()
        {
            return View();
        }
        #endregion

        #region service_provider(POST)
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
                    register.IsActive = false;
                    register.IsApproved = false;
                    register.Status = 0; //status 0 for false or inactive 

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
        #endregion

        #region LogOut
        public IActionResult LogOut()
        {
            HttpContext.Session.SetString("Userlogeddin", false.ToString());
            HttpContext.Session.Clear();
            TempData["LogOutMsg"] = "Logged out";
            return RedirectToAction("Index", "Home");
        }
        #endregion

        //[HttpPost]
        //#region LogOut
        //public IActionResult OnLogOut()
        //{
        //    HttpContext.Session.Clear();
        //    return PartialView("Index","Home");
        //}
        //#endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
