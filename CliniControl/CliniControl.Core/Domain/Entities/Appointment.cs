using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Domain.Entities
{
    public class Appointment
    {
        [Key]
        public Guid ID { get; set; }
        public Guid? PatientID { get; set; }

        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }

        public Guid? DentistID { get; set; }

        [ForeignKey("DentistID ")]
        public Dentist? Dentist { get; set; }
        public string? ProcedureType { get; set; }
        public string? Status { get; set; }
        public string? Comments { get; set; }
        public DateTime? AppointmentTime { get; set;}
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public double? Price { get; set; }
        public bool? Reminded { get; set; }
        public string? ExamsPath { get; set; }

    }
}
