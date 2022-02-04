using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Helperland.Models 
{
    public partial class ContactU
    {
        internal string subject;
        internal object body;

        public int ContactUsId { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Please Enter valid Email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select subject")]
        [MaxLength(50)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Write Message")]
        [MaxLength(80)]
        public string Message { get; set; }

        [MaxLength(100)]
        public string UploadFileName { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string FileName { get; set; }
    }
}
