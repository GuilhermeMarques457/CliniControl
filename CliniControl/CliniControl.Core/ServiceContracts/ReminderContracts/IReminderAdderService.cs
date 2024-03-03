using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.ReminderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.ReminderContracts
{
    public interface IReminderAdderService
    {
        Task<ReminderResponse> AddReminder(ReminderAddRequest? Reminder);
    }
}
