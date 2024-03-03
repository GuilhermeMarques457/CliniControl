using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ReminderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.ReminderContracts
{
    public interface IReminderGetterService
    {
        Task<List<ReminderResponse>?> GetReminderByDateTime(DateTime? date);
        Task<ReminderResponse?> GetReminderByID(Guid? ReminderID);
    }
}
