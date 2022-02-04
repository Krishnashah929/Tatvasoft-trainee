using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helperland.Data;
using Helperland.Models;
using System.Net.Mail;
using System.Net;

namespace Helperland.Controllers
{
    public class publicpagesController : Controller
    {
        private readonly HelperlandContext _helperlandContext;
    

        public publicpagesController(HelperlandContext helperlandContext)
        {
            _helperlandContext = helperlandContext;
        }
  
        public IActionResult FAQ()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CONTACT()
        {
            return View();
        }


        private void SendEmail( string body, string subject )
        {
            using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", "kdshah929@gmail.com"))
            {
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("krishnaa9121@gmail.com", "Kri$hn@91");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email is succcessfully sent.";
                }
            }
        }

        [HttpPost]
        public IActionResult CONTACT(ContactU contactU,String Lastname)
        {
            if (ModelState.IsValid)
            {
                using (HelperlandContext objHelperlandContext = new HelperlandContext())
                {
                    string lastname = Convert.ToString(Lastname);
                    contactU.Name = (contactU.Name + " " + lastname);
                    contactU.Message = ("the mail is sent by " + contactU.Name + "<br/> Email:  " + contactU.Email + "<br/> Phone number:  " + contactU.PhoneNumber + contactU.Message);
                    contactU.CreatedOn = DateTime.Now;
                    objHelperlandContext.ContactUs.Add(contactU);
                    objHelperlandContext.SaveChanges();
                    SendEmail(contactU.Message, contactU.Subject);
                    // Int64 id = objEmployee.EmployeeID;
                    ModelState.Clear();
                }
                // return View(objEmployee);
            }
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
    
    }
}
