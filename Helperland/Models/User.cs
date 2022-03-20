using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Helperland.Models
{
    public partial class User
    {
        public User()
        {
            FavoriteAndBlockedTargetUsers = new HashSet<FavoriteAndBlocked>();
            FavoriteAndBlockedUsers = new HashSet<FavoriteAndBlocked>();
            RatingRatingFromNavigations = new HashSet<Rating>();
            RatingRatingToNavigations = new HashSet<Rating>();
            ServiceRequestServiceProviders = new HashSet<ServiceRequest>();
            ServiceRequestUsers = new HashSet<ServiceRequest>();
            UserAddresses = new HashSet<UserAddress>();
        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Please Enter valid Email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        [MaxLength(50)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please repeat your password!")]
        [DataType(DataType.Password)]
        [MaxLength(50)]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [MaxLength(50)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile Number")]
        [MaxLength(50)]
        public string Mobile { get; set; }
        public int UserTypeId { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string UserProfilePicture { get; set; }
        public bool IsRegisteredUser { get; set; }
        public string PaymentGatewayUserRef { get; set; }

        public string ZipCode { get; set; }
        public bool WorksWithPets { get; set; }
        public int? LanguageId { get; set; }
        public int? NationalityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? Status { get; set; }
        public string BankTokenId { get; set; }
        public string TaxNo { get; set; }

        [NotMapped]
 
        public string Name { get; set; }

        [NotMapped]
 
        public decimal? ratings { get; set; }
        [NotMapped]
        public string AddressLine1 { get; set; }
        [NotMapped]
        public string AddressLine2 { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string PostalCode { get; set; }
        [NotMapped]
        public string Day { get; set; }
       [NotMapped]
        public string Month { get; set; }
       [NotMapped]
        public string Year { get; set; }

        [NotMapped]
        public string Comments { get; set; }
        //this line for the mailing services//
        public string ResetPasswordCode { get; set; }

        public virtual ICollection<FavoriteAndBlocked> FavoriteAndBlockedTargetUsers { get; set; }
        public virtual ICollection<FavoriteAndBlocked> FavoriteAndBlockedUsers { get; set; }
        public virtual ICollection<Rating> RatingRatingFromNavigations { get; set; }
        public virtual ICollection<Rating> RatingRatingToNavigations { get; set; }
        public virtual ICollection<ServiceRequest> ServiceRequestServiceProviders { get; set; }
        public virtual ICollection<ServiceRequest> ServiceRequestUsers { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        
    }
}
