using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Domain.Entities
{
    public class Clinic
    {
        [Key] public Guid ID { get; set; }
        public string? ClinicName { get; set; }
        public string? StreetName { get; set; }
        public string? Neighborhood { get; set; }
        public string? CNPJ { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public List<Dentist>? Dentists { get; set; }
        public List<Patient>? Patients { get; set; }
    }
}
