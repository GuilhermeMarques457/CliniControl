using CliniControl.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Domain.Entities
{
    public class Dentist
    {
        [Key] public Guid ID { get; set; }
        public string? DentistName { get; set; }

        public Guid ManagerID { get; set; }

        [ForeignKey("ManagerID")]
        public ApplicationUser? Manager { get; set; }
        public string? PhoneNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set;}
        public string? PhotoPath { get; set; }


    }
}
