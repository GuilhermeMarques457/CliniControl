using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Domain.Entities
{
    public class Reminder
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime? ActityDate { get; set; }
        public string ActivityDescription { get; set; } = null!;
        public bool Finished { get; set; }
        public string? ReminderType { get; set; }
    }
}
