using OdontoControl.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdontoControl.Core.Enums;

namespace OdontoControl.Core.Domain.Entities
{
    public class Patient
    {
        [Key] public Guid ID { get; set; }
        public string? PatientName { get; set; }

        public Guid ManagerID { get; set; }

        [ForeignKey("ManagerID")]
        public ApplicationUser? Manager { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? CPF { get; set; }
        public string? PhotoPath { get; set; }
    }
}
