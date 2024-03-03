using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Domain.Entities
{
    public class RequestedPatient
    {
        [Key] public Guid ID { get; set; }
        public string? PatientName { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? ClinicID { get; set; }
        public bool? Contacted { get; set; } = false;
    }
}
