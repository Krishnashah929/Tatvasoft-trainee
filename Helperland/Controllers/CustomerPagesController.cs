using Helperland.Data;
using Helperland.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Helperland.Models.ViewModel;
//status 1 is when provider complete that request
//status 2 is when provider/customer cancel that request
//status 3 is when  pending that request
//status 4 is when provider accept that request
//status 5 is when service date time is gone ane provider doesn't complete the service 
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
                TempData["Booknowfail"] = "Your Success Message";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult ValidZip(Zipcodeviewmodel ForZip)
        {
            HttpContext.Session.SetString("ForZip.ZipcodeValue", ForZip.ZipcodeValue);
            var Zipcode = _helperlandContext.Users.Where(z => z.ZipCode == ForZip.ZipcodeValue && z.UserTypeId == 2);
            if (Zipcode.Count() > 0)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }
<<<<<<< HEAD
     
=======
        //        int? Id = HttpContext.Session.GetInt32("userID");
        //            if (Id != null)
        //            {
        //                var Zipcode = _helperlandContext.Zipcodes.Where(x => x.ZipcodeValue == user.PostalCode);
        //                //HttpContext.Session.SetString("ForZip.ZipcodeValue", user.PostalCode);
        //                if (Zipcode.Count() > 0)
        //                {
        //                    return Ok(Json("true"));
        //                }
        //}
>>>>>>> c9491085483b751e39e6999dba7acf9d3122b592
        [HttpPost]
        public ActionResult Scheduledetails(Scheduledetails scheduledetails)
        {
            if (ModelState.IsValid)
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

            var result = _helperlandContext.UserAddresses.Where(x => x.UserId == Id && x.IsDeleted == false).ToList();
            foreach (var add in result)
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
            add.Status = 3;

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

            char[] extra = { '0', '0', '0', '0', '0' };
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
            if (result != null && serviceaddResult != null)
            {
                return Ok(Json(result.Entity.ServiceRequestId));
            }
            return Ok(Json("False"));
        }
        [HttpGet]
        public IActionResult Customer_Dashboard()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Customer_Setting()
        {
            return PartialView("_customerSettingPartial");
        }
        [HttpGet]
        public IActionResult Loaddashboard()
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {

                int id = (int)HttpContext.Session.GetInt32("userID");
                List<ServiceRequest> serviceRequest = ObjHelperlandContext.ServiceRequests.Where(x => x.UserId == id && (x.Status == 3 || x.Status == 4)).ToList();
                List<User> user = new List<User>();
                foreach (ServiceRequest users in serviceRequest)
                {
                    if (users.ServiceProviderId != null)
                    {
                        var providername = _helperlandContext.Users.Where(x => x.UserId == users.ServiceProviderId).FirstOrDefault();
                        users.Name = providername.FirstName + " " + providername.LastName;
                        var rate = _helperlandContext.Ratings.Where(c => c.ServiceRequestId == users.ServiceRequestId).ToList();
                        decimal temp = 0;
                        foreach (Rating rating in rate)
                        {
                            if (rating.Ratings != 0)
                            {
                                temp += rating.Ratings;
                            }
                        }
                        if (rate.Count() != 0)
                        {
                            temp /= rate.Count();
                        }
                        users.ratings = temp;
                    }
                    else
                    {
                        user.Add(null);
                    }
                }
                ViewBag.services = serviceRequest;
                return PartialView("_dashboardPartial");
            }
        }
        //for open service model onclick row
        [HttpGet]
        public IActionResult ServiceModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var details = ObjHelperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
                var address = ObjHelperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == id).FirstOrDefault();
                details.AddressLine1 = address.AddressLine1;
                details.AddressLine2 = address.AddressLine2;
                details.Mobile = address.Mobile;
                details.Email = address.Email;
                var extraservice = _helperlandContext.ServiceRequestExtras.Where(c => c.ServiceRequestId == id).FirstOrDefault();
                details.Extra = extraservice.ServiceExtraId;
                return PartialView("_CustomerServiceModelPartial", details);
            }
        }
        //to open reschedule modal
        [HttpGet]
        public IActionResult RescheduleServiceModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var details = ObjHelperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
                return PartialView("_CustomerReschedulePartial", details);
            }
        }
        //to open cancel modal
        [HttpGet]
        public IActionResult CancelServiceModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var details = ObjHelperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
                return PartialView("_CustomerCancelPartial", details);
            }
        }
        //for updating date and time
        [HttpPost]
        public IActionResult UpdateRescheduleeModel(ServiceRequest updatedate)
        {
            int? id = HttpContext.Session.GetInt32("userID");
            if (id != null)
            {
                ServiceRequest update = _helperlandContext.ServiceRequests.FirstOrDefault(x => x.ServiceRequestId == updatedate.ServiceRequestId);
                string date = updatedate.ServiceDate.ToString("yyyy-MM-dd");
                string time = updatedate.ServiceTime.ToString("HH:mm:ss");
                DateTime startDateTime = Convert.ToDateTime(date).Add(TimeSpan.Parse(time));
                update.ServiceStartDate = startDateTime;
                update.ModifiedDate = DateTime.Now;
                var result = _helperlandContext.ServiceRequests.Update(update);
                _helperlandContext.SaveChanges();
                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("false"));
            }
            return PartialView("_dashboardPartial");
        }
        //for canceling request
        [HttpPost]
        public IActionResult CancelRequestModel(ServiceRequest cancelreq)
        {
            int? id = HttpContext.Session.GetInt32("userID");
            if (id != null)
            {
                ServiceRequest cancel = _helperlandContext.ServiceRequests.FirstOrDefault(x => x.ServiceRequestId == cancelreq.ServiceRequestId);
                cancel.Status = 2;
                cancel.Comments = cancelreq.Comments;
                var result = _helperlandContext.ServiceRequests.Update(cancel);
                _helperlandContext.SaveChanges();
                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("false"));
            }
            return PartialView("_dashboardPartial");
        }
        //for get history rows
        [HttpGet]
        public IActionResult history()
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                int id = (int)HttpContext.Session.GetInt32("userID");
                List<ServiceRequest> serviceRequest = ObjHelperlandContext.ServiceRequests.Where(x => x.UserId == id && (x.Status == 1 || x.Status == 2)).ToList();
                List<User> user = new List<User>();
                foreach (ServiceRequest users in serviceRequest)
                {
                    if (users.ServiceProviderId != null)
                    {
                        var Name = _helperlandContext.Users.Where(x => x.UserId == users.ServiceProviderId).FirstOrDefault();
                        users.Name = Name.FirstName + " " + Name.LastName;
                        var rate = _helperlandContext.Ratings.Where(c => c.ServiceRequestId == users.ServiceRequestId).ToList();
                        decimal temp = 0;
                        foreach (Rating rating in rate)
                        {
                            if (rating.Ratings != 0)
                            {
                                temp += rating.Ratings;
                            }
                        }
                        if (rate.Count() != 0)
                        {
                            temp /= rate.Count();
                        }
                        users.ratings = temp;
                    }
                    else
                    {
                        user.Add(null);
                    }
                }
                ViewBag.history = serviceRequest;
                return PartialView("_customerHistoryPartial");
            }
        }
        //for get rating model
        [HttpGet]
        public IActionResult RatingProviderModel(int id)
        {
            var p = _helperlandContext.ServiceRequests.Where(c => c.ServiceRequestId == id).FirstOrDefault();
            var q = _helperlandContext.Users.Where(x => x.UserId == p.ServiceProviderId).FirstOrDefault();

            var rate = _helperlandContext.Ratings.Where(c => c.RatingTo == p.ServiceProviderId).ToList();
            var currentrate = _helperlandContext.Ratings.Where(c => c.ServiceRequestId == id).FirstOrDefault();

            decimal temp = 0;

            foreach (Rating rating in rate)
            {
                if (rating.Ratings != 0)
                {
                    temp += rating.Ratings;
                }
            }
            if (rate.Count() != 0)
            {
                temp /= rate.Count();
            }
            ViewBag.servicerequestid = id;
            ViewBag.serviceproviderid = p.ServiceProviderId;
            ViewBag.rating = temp;
            ViewBag.Name = q.FirstName + " " + q.LastName;

            return View("_RateProviderPartial", currentrate);

        }
        [HttpPost]
        public IActionResult AddRatings(Rating rating)
        {
            var userid = (int)HttpContext.Session.GetInt32("userID");
            var p = _helperlandContext.Ratings.Where(c => c.ServiceRequestId == rating.ServiceRequestId).FirstOrDefault();
            rating.Ratings = (rating.OnTimeArrival + rating.QualityOfService + rating.Friendly) / 3;
            rating.RatingFrom = userid;
            rating.RatingDate = DateTime.Now;

            if (p != null)
            {
                p.OnTimeArrival = rating.OnTimeArrival;
                p.QualityOfService = rating.QualityOfService;
                p.Friendly = rating.Friendly;
                p.Ratings = rating.Ratings;
                p.RatingDate = rating.RatingDate;
                p.RatingFrom = rating.RatingFrom;
                p.RatingTo = rating.RatingTo;
                p.Comments = rating.Comments;
                _helperlandContext.SaveChanges();

            }
            else
            {
                _helperlandContext.Ratings.Add(rating);
                _helperlandContext.SaveChanges();

            }
            return PartialView("_customerHistoryPartial");
        }
        [HttpGet]
        public IActionResult setting()
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                int id = (int)HttpContext.Session.GetInt32("userID");
                User ud = ObjHelperlandContext.Users.FirstOrDefault(x => x.UserId == id);
                List<UserAddress> address = ObjHelperlandContext.UserAddresses.Where(x => x.UserId == id && x.IsDeleted == false).ToList();
                ViewBag.details = ud;
                ViewBag.add = address;
                return PartialView("_customerSettingPartial");
            }
        }
        [HttpPost]
        public IActionResult Updateuserdetails(User user)
        {
            int? Id = HttpContext.Session.GetInt32("userID");
            if (Id != null)
            {
                User updated = _helperlandContext.Users.FirstOrDefault(x => x.UserId == Id);
                updated.FirstName = user.FirstName;
                updated.LastName = user.LastName;
                updated.Mobile = user.Mobile;
                updated.Email = user.Email;
                updated.ZipCode = user.PostalCode;
                var dob = user.Day + "-" + user.Month + "-" + user.Year;
                user.DateOfBirth = Convert.ToDateTime(dob);
                updated.LanguageId = user.LanguageId;
                updated.ModifiedDate = DateTime.Now;
                var result = _helperlandContext.Users.Update(updated);
                _helperlandContext.SaveChanges();
                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("False"));
            }
            return PartialView("_customerSettingPartial");
        }
        [HttpGet]
        public IActionResult EditAddressModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var editadd = ObjHelperlandContext.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
                return PartialView("_CustomerEditAddPArtial", editadd);
            }
        }
        [HttpGet]
        public IActionResult DelAddressModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var deladd = ObjHelperlandContext.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
                return PartialView("_CustomerDelAddPartial", deladd);
            }
        }
        [HttpPost]
        public IActionResult UpdateAddress(UserAddress newadd)
        {
            UserAddress updatedadd = _helperlandContext.UserAddresses.FirstOrDefault(x => x.AddressId == newadd.AddressId);
            updatedadd.AddressLine1 = newadd.AddressLine1;
            updatedadd.AddressLine2 = newadd.AddressLine2;
            updatedadd.PostalCode = newadd.PostalCode;
            updatedadd.State = newadd.State;
            updatedadd.Mobile = newadd.Mobile;
            updatedadd.IsDefault = true;
            var result = _helperlandContext.UserAddresses.Update(updatedadd);
            _helperlandContext.SaveChanges();
<<<<<<< HEAD

            User users = _helperlandContext.Users.FirstOrDefault(x => x.UserId == updatedadd.UserId);
            users.ZipCode = updatedadd.PostalCode;
            var results = _helperlandContext.Users.Update(users);
            _helperlandContext.SaveChanges();

=======
>>>>>>> c9491085483b751e39e6999dba7acf9d3122b592
            if (result != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("False"));
<<<<<<< HEAD
 
=======
>>>>>>> c9491085483b751e39e6999dba7acf9d3122b592
        }
        [HttpPost]
        public IActionResult DeleteAddress(UserAddress deladd)
        {
            int? id = HttpContext.Session.GetInt32("userID");
            if (id != null)
            {
                UserAddress cancel = _helperlandContext.UserAddresses.FirstOrDefault(x => x.AddressId == deladd.AddressId);
                cancel.IsDeleted = true;
                var result = _helperlandContext.UserAddresses.Update(cancel);
                _helperlandContext.SaveChanges();
                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("false"));
            }
            return PartialView("_customerSettingPartial");
        }
        [HttpPost]
        public IActionResult ChangePassword(password pass)
        {
            var userid = (int)HttpContext.Session.GetInt32("userID");
            if (userid != null)
            {
                User user = _helperlandContext.Users.FirstOrDefault(x => x.UserId == userid);
                if (user.Password == pass.Password)
                {
                    user.Password = pass.NewPassword;
                    _helperlandContext.Users.Update(user);
                    _helperlandContext.SaveChanges();
                }
            }
            return PartialView("_SPsettingPartial");
        }
    }
}

