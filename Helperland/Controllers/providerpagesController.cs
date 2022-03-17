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
    public class providerpagesController : Controller
    {
        private readonly HelperlandContext _helperlandContext;

        public providerpagesController(HelperlandContext helperlandContext)
        {
            _helperlandContext = helperlandContext;
        }
        [HttpGet]
        public IActionResult Provider_Dashboard()
        {
            return View();
        }

        //Provider new service request
        [HttpGet]
        public IActionResult SpNewRequest()
        {
            List<ServiceRequest> serviceRequest = _helperlandContext.ServiceRequests.Where(x => x.Status == 3).ToList();
            List<User> user = new List<User>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var customername = _helperlandContext.Users.Where(x => x.UserId == users.UserId).FirstOrDefault();
                users.Name = customername.FirstName + " " + customername.LastName;
            }
            List<ServiceRequestAddress> address = new List<ServiceRequestAddress>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var addresses = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == users.ServiceRequestId).FirstOrDefault();
                users.AddressLine1 = addresses.AddressLine1;
                users.AddressLine2 = addresses.AddressLine2;
                users.City = addresses.City;
                users.ZipCode = addresses.PostalCode;
            }
            ViewBag.services = serviceRequest;
            return PartialView("_SPdashboardPartial");
        }
        [HttpGet]
        public IActionResult OpenNewServiceModel(int id)
        {
            var details = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            var address = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            details.AddressLine1 = address.AddressLine1;
            details.AddressLine2 = address.AddressLine2;
            details.Mobile = address.Mobile;
            details.Email = address.Email;
            var extraservice = _helperlandContext.ServiceRequestExtras.Where(c => c.ServiceRequestId == id).FirstOrDefault();
            details.Extra = extraservice.ServiceExtraId;
            return PartialView("_OpenNewReqModelPartial", details);
        }
        [HttpPost]
        public IActionResult AcceptNewRequest(int id)
        {
            int? proid = HttpContext.Session.GetInt32("userID");

            ServiceRequest accept = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            accept.Status = 4;
            accept.ServiceProviderId = proid;
            var result = _helperlandContext.ServiceRequests.Update(accept);
            _helperlandContext.SaveChanges();
            if (result != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }

        //Provider upcoming service
        [HttpGet]
        public IActionResult SpUpcomingRequest()
        {
            int? proid = HttpContext.Session.GetInt32("userID");
            List<ServiceRequest> serviceRequest = _helperlandContext.ServiceRequests.Where(x => x.Status == 4 && x.ServiceProviderId == proid).ToList();

            List<User> user = new List<User>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var customername = _helperlandContext.Users.Where(x => x.UserId == users.UserId).FirstOrDefault();
                users.Name = customername.FirstName + " " + customername.LastName;

            }
            List<ServiceRequestAddress> address = new List<ServiceRequestAddress>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var addresses = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == users.ServiceRequestId).FirstOrDefault();
                users.AddressLine1 = addresses.AddressLine1;
                users.AddressLine2 = addresses.AddressLine2;
                users.City = addresses.City;
                users.ZipCode = addresses.PostalCode;
            }
            ViewBag.services = serviceRequest;
            return PartialView("_SPupcomingPartial");
        }
        [HttpGet]
        public IActionResult OpenUpComingModel(int id)
        {
            var details = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            var address = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            details.AddressLine1 = address.AddressLine1;
            details.AddressLine2 = address.AddressLine2;
            details.Mobile = address.Mobile;
            details.Email = address.Email;
            var extraservice = _helperlandContext.ServiceRequestExtras.Where(c => c.ServiceRequestId == id).FirstOrDefault();
            details.Extra = extraservice.ServiceExtraId;
            return PartialView("_OpenUpcomingModelPartial", details);
        }
        [HttpPost]
        public IActionResult CompleteRequest(int id)
        {
            int? proid = HttpContext.Session.GetInt32("userID");

            ServiceRequest complete = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            complete.Status = 1;
            complete.ServiceProviderId = proid;
            var result = _helperlandContext.ServiceRequests.Update(complete);
            _helperlandContext.SaveChanges();
            if (result != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }
        [HttpPost]
        public IActionResult CancelRequest(int id)
        {
            int? proid = HttpContext.Session.GetInt32("userID");

            ServiceRequest cancel = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            cancel.Status = 3;
            cancel.ServiceProviderId = proid;
            var result = _helperlandContext.ServiceRequests.Update(cancel);
            _helperlandContext.SaveChanges();
            if (result != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }

        //Provider history service
        [HttpGet]
        public IActionResult SpHistory()
        {
            int? proid = HttpContext.Session.GetInt32("userID");
            List<ServiceRequest> serviceRequest = _helperlandContext.ServiceRequests.Where(x => x.Status == 1 && x.ServiceProviderId == proid).ToList();
            List<User> user = new List<User>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var customername = _helperlandContext.Users.Where(x => x.UserId == users.UserId).FirstOrDefault();
                users.Name = customername.FirstName + " " + customername.LastName;
            }
            List<ServiceRequestAddress> address = new List<ServiceRequestAddress>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var addresses = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == users.ServiceRequestId).FirstOrDefault();
                users.AddressLine1 = addresses.AddressLine1;
                users.AddressLine2 = addresses.AddressLine2;
                users.City = addresses.City;
                users.ZipCode = addresses.PostalCode;
            }
            ViewBag.services = serviceRequest;
            return PartialView("_SPhistoryPartial");
        }
        [HttpGet]
        public IActionResult OpenHistoryModel(int id)
        {
            var details = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            var address = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            details.AddressLine1 = address.AddressLine1;
            details.AddressLine2 = address.AddressLine2;
            details.Mobile = address.Mobile;
            details.Email = address.Email;
            var extraservice = _helperlandContext.ServiceRequestExtras.Where(c => c.ServiceRequestId == id).FirstOrDefault();
            details.Extra = extraservice.ServiceExtraId;
            return PartialView("_OpenHistoryModelPartial", details);
        }

        //Provider Rating
        [HttpGet]
        public IActionResult SpRating()
        {
            int? proid = HttpContext.Session.GetInt32("userID");
            List<ServiceRequest> serviceRequest = _helperlandContext.ServiceRequests.Where(x => x.Status == 1 && x.ServiceProviderId == proid).ToList();
            List<User> user = new List<User>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var customername = _helperlandContext.Users.Where(x => x.UserId == users.UserId).FirstOrDefault();
                users.Name = customername.FirstName + " " + customername.LastName;
<<<<<<< HEAD
=======
               
>>>>>>> c9491085483b751e39e6999dba7acf9d3122b592
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
            ViewBag.services = serviceRequest;
            return PartialView("_SPratingPartial");
        }

        //Provider Block
        [HttpGet]
        public IActionResult SpBlock()
        {
            int? userid = HttpContext.Session.GetInt32("userID");
            var p = _helperlandContext.ServiceRequests.Where(c => c.ServiceProviderId == userid && c.Status == 1).AsEnumerable().GroupBy(x => x.UserId).ToList();
            List<User> users = new List<User>();
            List<string> blocklist = new List<string>();
            foreach (var i in p)
            {
                User temp = _helperlandContext.Users.Find(i.Key);
                users.Add(temp);

                var blockornot = _helperlandContext.FavoriteAndBlockeds.Where(c => c.UserId == userid && c.TargetUserId == i.Key).FirstOrDefault();
                if (blockornot != null)
                {
                    if (blockornot.IsBlocked == true)
                    {
                        string value = "block";
                        blocklist.Add(value);
                    }
                    else
                    {
                        string value = "unblock";
                        blocklist.Add(value);
                    }
                }
                else
                {
                    string value = "unblock";
                    blocklist.Add(value);
                }
            }
            ViewBag.blockedornot = blocklist;
            return PartialView("_SPblockPartial", users);
        }

        [HttpPost]
        public IActionResult BlockUser(int id)
        {
            int? userid = HttpContext.Session.GetInt32("userID");
            var p = _helperlandContext.FavoriteAndBlockeds.Where(c => c.UserId == userid && c.TargetUserId == id).FirstOrDefault();
            if (p == null)
            {
                FavoriteAndBlocked favoriteAndBlocked = new FavoriteAndBlocked
                {
                    UserId = (int)userid,
                    TargetUserId = id,
                    IsBlocked = true,
                    IsFavorite = false
                };
                _helperlandContext.FavoriteAndBlockeds.Add(favoriteAndBlocked);
                _helperlandContext.SaveChanges();
            }
            else
            {
                p.IsBlocked = true;
                _helperlandContext.SaveChanges();
            }
            return PartialView("_SPblockPartial");
        }

        [HttpPost]
        public IActionResult UnBlockUser(int id)
        {
            int? userid = HttpContext.Session.GetInt32("userID");
            var p = _helperlandContext.FavoriteAndBlockeds.Where(c => c.UserId == userid && c.TargetUserId == id).FirstOrDefault();
            if (p != null)
            {
                p.IsBlocked = false;
                _helperlandContext.SaveChanges();
            }
            return PartialView("_SPblockPartial");
        }
        //Provider Setting
        [HttpGet]
        public IActionResult SpSetting()
        {
            int id = (int)HttpContext.Session.GetInt32("userID");
            User ud = _helperlandContext.Users.FirstOrDefault(x => x.UserId == id);
            UserAddress address = _helperlandContext.UserAddresses.Where(x => x.UserId == id).FirstOrDefault();
            if (address != null)
            {
                ViewBag.add = address;
            }
<<<<<<< HEAD
           
=======
>>>>>>> c9491085483b751e39e6999dba7acf9d3122b592
            ViewBag.details = ud;
            return PartialView("_SPsettingPartial");
        }
        [HttpPost]
        public IActionResult Updateproviderdetails(User user)
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
                updated.Gender = user.Gender;
                updated.NationalityId = user.NationalityId;
                if (user.Day != null && user.Month != null && user.Year != null)
                {
                    var dob = user.Day + "-" + user.Month + "-" + user.Year;
                    updated.DateOfBirth = Convert.ToDateTime(dob);
                }
                updated.UserProfilePicture = user.UserProfilePicture;
                updated.ModifiedDate = DateTime.Now;
                var result = _helperlandContext.Users.Update(updated);
                _helperlandContext.SaveChanges();

                var address = _helperlandContext.UserAddresses.Where(x => x.UserId == Id).FirstOrDefault();
                if (address != null)
                {
                    address.AddressLine1 = user.AddressLine1;
                    address.AddressLine2 = user.AddressLine2;
                    address.City = user.City;
                    address.PostalCode = user.PostalCode;
                    var serviceaddResult = _helperlandContext.UserAddresses.Update(address);
                    _helperlandContext.SaveChanges();
                }
                else
                {
                    UserAddress serviceadd = new UserAddress();
                    serviceadd.UserId = (int)Id;
                    serviceadd.AddressLine1 = user.AddressLine1;
                    serviceadd.AddressLine2 = user.AddressLine2;
                    serviceadd.City = user.City;
                    serviceadd.Mobile = user.Mobile;
                    serviceadd.PostalCode = user.PostalCode;
                    var serviceaddResult = _helperlandContext.UserAddresses.Add(serviceadd);
                    _helperlandContext.SaveChanges();
                }
            }
            return PartialView("_SPsettingPartial");
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



