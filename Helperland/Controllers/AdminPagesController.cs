using Helperland.Data;
using Helperland.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    public class AdminPagesController : Controller
    {
        private readonly HelperlandContext _helperlandContext;

        public AdminPagesController(HelperlandContext helperlandContext)
        {
            _helperlandContext = helperlandContext;
        }
        [HttpGet]
        public IActionResult Admin_Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UserManagement()
        {
            // int? id = HttpContext.Session.GetInt32("userID");
            List<User> useradmin = _helperlandContext.Users.Where(x => x.UserTypeId == 1 || x.UserTypeId == 2).ToList();


            ViewBag.services = useradmin;
            return PartialView("User_Management_Partial");
        }
        [HttpGet]
        public IActionResult UserRequest()
        {
            List<ServiceRequest> serviceRequest = _helperlandContext.ServiceRequests.ToList();
            List<User> user = new List<User>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var customername = _helperlandContext.Users.Where(x => x.UserId == users.UserId).FirstOrDefault();
                users.custName = customername.FirstName + " " + customername.LastName;
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
            return PartialView("User_Request_Partial");
        }
        public IActionResult ApproveUser(int id)
        {
            User user = _helperlandContext.Users.Where(x => x.UserId == id).FirstOrDefault();
            return PartialView("Approve_User_Partial", user);
        }

        public IActionResult FinalApproveUser(User user)
        {
            User users = _helperlandContext.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            users.IsActive = true;
            users.IsApproved = true;
            users.Status = 1;
            var aprroveuser = _helperlandContext.Users.Update(users);
            _helperlandContext.SaveChanges();
            if (aprroveuser != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }

        public IActionResult DeactivateUser(int id)
        {
            User users = _helperlandContext.Users.Where(x => x.UserId == id).FirstOrDefault();
            users.IsActive = false;
            users.Status = 0;
            var aprroveuser = _helperlandContext.Users.Update(users);
            _helperlandContext.SaveChanges();
            if (aprroveuser != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }
        public IActionResult ActivateUser(int id)
        {
            User users = _helperlandContext.Users.Where(x => x.UserId == id).FirstOrDefault();
            users.IsActive = true;
            users.Status = 1;
            var aprroveuser = _helperlandContext.Users.Update(users);
            _helperlandContext.SaveChanges();
            if (aprroveuser != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));
        }
    }
}
