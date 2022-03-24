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
        public IActionResult UserManagement(SearchUser data)
        {
            List<User> useradmin = _helperlandContext.Users.Where(x => x.UserTypeId == 1 || x.UserTypeId == 2).ToList();
            useradmin = useradmin.Where(x => (string.IsNullOrEmpty(data.Name) || x.FirstName.ToLower().Contains(data.Name.ToLower())) &&
                                                   (string.IsNullOrEmpty(data.Mobile) || x.Mobile.ToLower().Contains(data.Mobile.ToLower())) &&
                                                   (string.IsNullOrEmpty(data.ZipCode) || (!string.IsNullOrEmpty(x.ZipCode) ? x.ZipCode.ToLower().Contains(data.ZipCode.ToLower()) : false)) &&
                                                   (!data.UserTypeId.HasValue || x.UserTypeId == data.UserTypeId) && (!data.CreatedDate.HasValue || x.CreatedDate.Date == data.CreatedDate.Value.Date)).ToList();
            ViewBag.data = useradmin;
            return PartialView("User_Management_Partial");
        }
        [HttpGet]
        public IActionResult UserRequest(SearchUser data)
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
                users.AddressLine2 = addresses.AddressLine2;
                users.AddressLine1 = addresses.AddressLine1;
                users.City = addresses.City;
                users.ZipCode = addresses.PostalCode;
            }
            if (serviceRequest.Count > 0)
            {
                serviceRequest = serviceRequest.Where(x => (!data.ServiceRequestId.HasValue || x.ServiceRequestId == data.ServiceRequestId) &&
                (string.IsNullOrEmpty(data.custName) || x.custName.ToLower().Contains(data.custName.ToLower())) &&
                                                  (string.IsNullOrEmpty(data.Name) || (!string.IsNullOrEmpty(x.Name) ? x.Name.ToLower().Contains(data.Name.ToLower()) : false)) &&
                                                  (string.IsNullOrEmpty(data.ZipCode) || (!string.IsNullOrEmpty(x.ZipCode) ? x.ZipCode.ToLower().Contains(data.ZipCode.ToLower()) : false)) &&
                                                  (!data.Status.HasValue || x.Status == data.Status) &&
                                                  (!data.ServiceStartDate.HasValue || x.ServiceStartDate.Date == data.ServiceStartDate.Value.Date)).ToList();
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
        public IActionResult EditServiceModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var editadd = ObjHelperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();

                ServiceRequestAddress address = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == id).FirstOrDefault();
                editadd.AddressLine1 = address.AddressLine1;
                editadd.AddressLine2 = address.AddressLine2;
                editadd.ZipCode = address.PostalCode;
                editadd.City = address.City;

                return PartialView("EditRequestModelPartial", editadd);
            }
        }

        [HttpPost]
        public IActionResult UpdateServiceModel(ServiceRequest data)
        {
            ServiceRequest update = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == data.ServiceRequestId).FirstOrDefault();

            string date = data.ServiceDate.ToString("yyyy-MM-dd");
            string time = data.ServiceTime.ToString("HH:mm:ss");
            DateTime startDateTime = Convert.ToDateTime(date).Add(TimeSpan.Parse(time));
            update.ServiceStartDate = startDateTime;
            update.ModifiedDate = DateTime.Now;
            update.Comments = data.Comments;
            var result = _helperlandContext.ServiceRequests.Update(update);
            _helperlandContext.SaveChanges();
            if (result != null)
            {
                return Ok(Json("true"));
            }
            return Ok(Json("false"));

            ServiceRequestAddress updateadd = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == data.ServiceRequestId).FirstOrDefault();
            updateadd.AddressLine1 = data.AddressLine1;
            updateadd.AddressLine2 = data.AddressLine2;
            updateadd.PostalCode = data.ZipCode;
            updateadd.City = data.City;
            var result1 = _helperlandContext.ServiceRequestAddresses.Update(updateadd);
            _helperlandContext.SaveChanges();
            return PartialView("User_Request_Partial");
        }

        [HttpGet]
        public IActionResult CancelServiceModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var details = ObjHelperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
                return PartialView("CancelRequestModelPartial", details);
            }
        }
        [HttpPost]
        public IActionResult CancelRequestModel(ServiceRequest cancelreq)
        {
            int? id = HttpContext.Session.GetInt32("userID");
            if (id != null)
            {
                ServiceRequest cancel = _helperlandContext.ServiceRequests.FirstOrDefault(x => x.ServiceRequestId == cancelreq.ServiceRequestId);
                cancel.Status = 3;
                cancel.Comments = cancelreq.Comments;
                cancel.ServiceProviderId = null;
                var result = _helperlandContext.ServiceRequests.Update(cancel);
                _helperlandContext.SaveChanges();
                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("false"));
            }
            return PartialView("User_Request_Partial");
        }
    }
}