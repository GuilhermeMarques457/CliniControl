using OdontoControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.Enums;

namespace OdontoControl.Core.DTO.ReminderDTO
{
    public class ReminderResponse
    {
        public Guid ID { get; set; }
        public DateTime? ActityDate { get; set; }
        public string ActivityDescription { get; set; } = null!;
        public bool Finished { get; set; }
        public ReminderTypeOptions? ReminderType { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(ReminderResponse)) return false;

            ReminderResponse Reminder = (ReminderResponse)obj;

            return ID == Reminder.ID &&
                ActivityDescription == Reminder.ActivityDescription &&
                Finished == Reminder.Finished &&
                ActivityDescription == Reminder.ActivityDescription &&
                ReminderType == Reminder.ReminderType;
              
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public ReminderUpdateRequest ToPatientUpdateRequest()
        {
            return new ReminderUpdateRequest
            {
                ID = ID,
                ActityDate = ActityDate,
                ActivityDescription = ActivityDescription,
                Finished = Finished,
                ReminderType = ReminderType
            };
        }

        public Reminder ToReminder()
        {
            return new Reminder()
            {
                ActityDate = ActityDate,
                ActivityDescription = ActivityDescription,
                Finished = Finished,
                ID = ID,
                ReminderType = ReminderType.ToString()
            };
        }
    }

    public static class ReminderExtensions
    {
        public static ReminderResponse ToReminderResponse(this Reminder Reminder)
        {
            return new ReminderResponse
            {
                ActityDate = Reminder.ActityDate,
                ActivityDescription = Reminder.ActivityDescription,
                Finished = Reminder.Finished,
                ID = Reminder.ID,
                ReminderType = Enum.TryParse(Reminder.ReminderType, true, out ReminderTypeOptions reminderType) ? reminderType : null,
            };
        }
    }
}
