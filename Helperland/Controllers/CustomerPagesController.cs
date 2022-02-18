using Helperland.Data;
using Helperland.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Helperland.Controllers
{
    public class CustomerPagesController : Controller
    {
        private readonly HelperlandContext _helperlandContext;
        public CustomerPagesController(HelperlandContext helperlandContext)
        {
            _helperlandContext = helperlandContext;
        }
        [HttpGet]
        public IActionResult BOOK_NOW()
        {
            string loggedin = HttpContext.Session.GetString("Userlogeddin");

            if (loggedin == "true")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        [HttpPost]
        public ActionResult ValidZip(Zipcodeviewmodel ForZip)
        {
             
            HttpContext.Session.SetString("ZipcodeValue", ForZip.ZipcodeValue);
            var Zipcode = _helperlandContext.Zipcodes.Where(z => z.ZipcodeValue == ForZip.ZipcodeValue);
            if(Zipcode.Count() >0)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }
        [HttpPost]
        public ActionResult Scheduledetails(Scheduledetails scheduledetails)
        {
            if(ModelState.IsValid) 
            {
                return Ok(Json("true"));
            }
            else
            {
                return Ok(Json("false"));
            }
        }
        [HttpGet]
        public JsonResult LoadAddress()
        {
            List<UserAddress> Address = new List<UserAddress>();
            int? Id = HttpContext.Session.GetInt32("userID");
            var result = _helperlandContext.UserAddresses.Where(x => x.UserId == Id).ToList();
            foreach(var add in result)
            {
                UserAddress useraddress = new UserAddress();
                
                useraddress.AddressId = add.AddressId;
                useraddress.AddressLine1 = add.AddressLine1;
                useraddress.AddressLine2 = add.AddressLine2;
                useraddress.City = add.City;
                useraddress.PostalCode = add.PostalCode;
                useraddress.IsDefault = add.IsDefault;
                useraddress.Mobile = add.Mobile;
                Address.Add(useraddress);
            }
            return new JsonResult(Address);
        }

        [HttpPost]
        public ActionResult NewAddress(UserAddress newuseradd)
        {
            int? Id = HttpContext.Session.GetInt32("userID");
            newuseradd.UserId = (int)Id;
            newuseradd.IsDefault = false;
            newuseradd.IsDeleted = false;
            User user = _helperlandContext.Users.Where(x => x.UserId == Id).FirstOrDefault();
            newuseradd.Email = user.Email;
            var result = _helperlandContext.UserAddresses.Add(newuseradd);
            _helperlandContext.SaveChanges();
            if (result != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("False"));
        }

        [HttpPost]
        public ActionResult Completebooking(Completebooking Finalbooking)
        {
            int Id = (int)HttpContext.Session.GetInt32("userID");
            ServiceRequest add = new ServiceRequest();
            add.UserId = Id;
            add.ServiceId = Id;
            //add.ServiceStartDate = Finalbooking.ServiceStartDate;
            add.ZipCode = Finalbooking.ZipcodeValue;
            add.ServiceHourlyRate = 25;
            add.ServiceHours = Finalbooking.ServiceHours;
            add.ExtraHours = Finalbooking.ExtraHours;
            add.SubTotal = (decimal)(add.ServiceHours + add.ExtraHours);
            add.TotalCost = (decimal)(add.SubTotal * add.ServiceHourlyRate);
            add.Comments = Finalbooking.Comments;
            add.PaymentDue = false;
            add.PaymentDone = true;
            add.HasPets = Finalbooking.HasPets;
            add.CreatedDate = DateTime.Now;
            add.ModifiedDate = DateTime.Now;
            add.HasIssue = false;

            string date = Finalbooking.ServiceDate.ToString("yyyy-MM-dd");
            string time = Finalbooking.ServiceTime.ToString("HH:mm:ss");
            DateTime startDateTime = Convert.ToDateTime(date).Add(TimeSpan.Parse(time));
            add.ServiceStartDate = startDateTime;
            var result = _helperlandContext.ServiceRequests.Add(add);
            _helperlandContext.SaveChanges();
            int id = add.ServiceRequestId;

            UserAddress UserAddress = _helperlandContext.UserAddresses.Where(x => x.AddressId == Finalbooking.AddressId).FirstOrDefault();

            ServiceRequestAddress serviceadd = new ServiceRequestAddress();
            serviceadd.AddressLine1 = UserAddress.AddressLine1;
            serviceadd.AddressLine2 = UserAddress.AddressLine2;
            serviceadd.City = UserAddress.City;
            serviceadd.Email = UserAddress.Email;
            serviceadd.Mobile = UserAddress.Mobile;
            serviceadd.PostalCode = UserAddress.PostalCode;
            serviceadd.ServiceRequestId = id;
            serviceadd.State = UserAddress.State;
            var serviceaddResult = _helperlandContext.ServiceRequestAddresses.Add(serviceadd);
            _helperlandContext.SaveChanges();

            char []extra = {'0','0','0','0','0'};
            if (Finalbooking.Cabinet == true)
            {
                extra[0] = '1';
            }
            if (Finalbooking.Oven == true)
            {
                extra[1] = '1';
            }
            if (Finalbooking.Window == true)
            {
                extra[2] = '1';
            }
            if (Finalbooking.Fridge == true)
            {
                extra[3] = '1';
            }
            if (Finalbooking.Laundry == true)
            {
                extra[4] = '1';
            }
            string etc = new string(extra);
            int extraids = Convert.ToInt32(etc);
            ServiceRequestExtra extraservice = new ServiceRequestExtra();
            extraservice.ServiceRequestId = id;
            extraservice.ServiceExtraId = extraids;
            _helperlandContext.ServiceRequestExtras.Add(extraservice);
            _helperlandContext.SaveChanges();
            if ( result != null && serviceaddResult != null)
            {
                return Ok(Json(result.Entity.ServiceRequestId));
            }
            return Ok(Json("False"));       }

    }
}


