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
using System.Net.Mail;
using System.Net;
using MimeKit;

//status 1 is when provider complete that request
//status 2 is when provider/customer cancel that request
//status 3 is when  pending that request
//status 4 is when provider accept that request
//status 5 is when service date time is gone ane provider doesn't complete the service.
//Samarthprovider = 364003
//DwitiProvider & MayankProvider = 364001
//ShwetaProvider = 364002

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
            int? proid = HttpContext.Session.GetInt32("userID");
            User SP = _helperlandContext.Users.Where(x => x.UserId == proid).FirstOrDefault();
            List<ServiceRequest> serviceRequest = new List<ServiceRequest>();

            serviceRequest = _helperlandContext.ServiceRequests.Where(x => x.Status == 3 && x.ZipCode == SP.ZipCode).ToList();

            List<User> user = new List<User>();
            foreach (ServiceRequest users in serviceRequest)
            {
                var FB = _helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == proid && x.IsBlocked == true).ToList();
            
                var SPBlock = _helperlandContext.FavoriteAndBlockeds.Where(x => x.TargetUserId == proid && x.IsBlocked == true).ToList();
                
                if (FB != null)
                { 
                    foreach(FavoriteAndBlocked Fbblock in FB)
                    {
                        serviceRequest = serviceRequest.Where(x => x.UserId != Fbblock.TargetUserId).ToList();
                    }
                }
                if (SPBlock != null)
                {
                    foreach (FavoriteAndBlocked SPBlocks in SPBlock)
                    {
                        serviceRequest = serviceRequest.Where(x => x.UserId != SPBlocks.UserId).ToList();
                    }
                }
            
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
            List<ServiceRequest> acceptreq = _helperlandContext.ServiceRequests.Where(x => x.ServiceProviderId == proid && x.Status == 4 && DateTime.Compare(x.ServiceStartDate.Date, accept.ServiceStartDate.Date) == 0).ToList();
           
            TimeSpan RescheduleStart = accept.ServiceStartDate.TimeOfDay;
            TimeSpan RescheduleEnd = accept.ServiceStartDate.AddHours(Convert.ToDouble(accept.SubTotal)).TimeOfDay;

            foreach (var timeconflict in acceptreq)
            {
                TimeSpan AcceptedStart = timeconflict.ServiceStartDate.TimeOfDay;
                TimeSpan AcceptedEnd = timeconflict.ServiceStartDate.AddHours(Convert.ToDouble(timeconflict.SubTotal)).TimeOfDay;

                if ((RescheduleStart >= AcceptedStart && RescheduleStart <= AcceptedEnd) ||
                    (RescheduleEnd >= AcceptedStart && RescheduleEnd <= AcceptedEnd))
                {
                    return Ok(Json("false"));
                }
            }


            accept.Status = 4;
            accept.ServiceProviderId = proid;
            var result = _helperlandContext.ServiceRequests.Update(accept);
            _helperlandContext.SaveChanges();

            //for mailing service on accept service.

            List<User> users = _helperlandContext.Users.Where(x => x.UserTypeId == 2 && x.UserId != proid && x.ZipCode ==  accept.ZipCode ).ToList();
 
            foreach (User sps in users)
            {
                var subject = "Service is already aceepted by someone.";
                var body = "Hi " + sps.FirstName + " " + sps.LastName + "<br/> The service request Id : " + accept.ServiceRequestId + " has already been accepted by other service provider .";
                SendEmail(sps.Email, body, subject);
            }

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
        [HttpGet]
        public IActionResult CancelServiceModel(int id)
        {
            using (HelperlandContext ObjHelperlandContext = new HelperlandContext())
            {
                var details = ObjHelperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
                return PartialView("_CancelRequestModelPartial", details);
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
                var result = _helperlandContext.ServiceRequests.Update(cancel);
                _helperlandContext.SaveChanges();
                if (result != null)
                {
                    return Ok(Json("true"));
                }
                return Ok(Json("false"));
            }
            return PartialView("_SPupcomingPartial");
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
                users.Comments = customername.Comments;
                var rate = _helperlandContext.Ratings.Where(c => c.ServiceRequestId == users.ServiceRequestId).ToList();
                decimal temp = 0;

                foreach (Rating rating in rate)
                {
                    if (rating.Ratings != 0)
                    {
                        temp += rating.Ratings;
                        users.Comments = rating.Comments;
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
            return SpBlock();
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
            return SpBlock();
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
            if (ud.DateOfBirth != null)
            {
                DateTime DOB = Convert.ToDateTime(ud.DateOfBirth.ToString());
                ud.Day = DOB.Day.ToString();
                ud.Month = DOB.Month.ToString();
                ud.Year = DOB.Year.ToString();
            }
            ViewBag.details = ud;
            return PartialView("_SPsettingPartial" , ud);
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
                updated.ZipCode = user.PostalCode;
                updated.Gender = user.Gender;
                updated.NationalityId = user.NationalityId;
               
                if (user.Day != null && user.Month != null && user.Year != null)
                {
                    var dob = user.Month + "-" + user.Day + "-" + user.Year;
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

        //for sending mail after accept the request
        private void SendEmail(string email, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", email))
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
                    ViewBag.Message = "Email is succcessfully sent to Providers .";
                }
            }
        }
    }
}



