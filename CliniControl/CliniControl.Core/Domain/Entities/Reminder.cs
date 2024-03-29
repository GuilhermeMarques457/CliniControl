﻿using CliniControl.Core.Domain.IdentityEntities;
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
