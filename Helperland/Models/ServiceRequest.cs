﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Helperland.Models
{
    public partial class ServiceRequest
    {
        public ServiceRequest()
        {
            Ratings = new HashSet<Rating>();
            ServiceRequestAddresses = new HashSet<ServiceRequestAddress>();
            ServiceRequestExtras = new HashSet<ServiceRequestExtra>();
        }

        public int ServiceRequestId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public string ZipCode { get; set; }
        public decimal? ServiceHourlyRate { get; set; }
        public double ServiceHours { get; set; }
        public double ExtraHours { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal TotalCost { get; set; }
        public string Comments { get; set; }
        public string PaymentTransactionRefNo { get; set; }
        public bool PaymentDue { get; set; }
        public int? ServiceProviderId { get; set; }
        public DateTime? SpacceptedDate { get; set; }
        public bool HasPets { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public decimal? RefundedAmount { get; set; }
        public decimal Distance { get; set; }
        public bool? HasIssue { get; set; }
        public bool? PaymentDone { get; set; }
        public Guid? RecordVersion { get; set; }

        [NotMapped]
        public int Extra { get; set; }

        [NotMapped]
        public bool Extra1 { get; set; }
      
        [NotMapped]
        public bool Extra2 { get; set; }
        [NotMapped]
        public bool Extra3 { get; set; }
        [NotMapped]
        public bool Extra4 { get; set; }
        [NotMapped]
        public bool Extra5 { get; set; }
        [NotMapped]
        public DateTime ServiceDate { get; set; }
        [NotMapped]
        public DateTime ServiceTime { get; set; }
        [NotMapped]
        public string AddressLine1 { get; set; }
        [NotMapped]
        public string AddressLine2 { get; set; }
        [NotMapped]
        public string Mobile { get; set; }
        [NotMapped]
        public string Email { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string custName { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string PostalCode { get; set; }
        [NotMapped]
        public decimal? ratings { get; set; }
      


        [NotMapped]
        public int UserTypeId { get; set; }

        public virtual User ServiceProvider { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ServiceRequestAddress> ServiceRequestAddresses { get; set; }
        public virtual ICollection<ServiceRequestExtra> ServiceRequestExtras { get; set; }
    }
}
