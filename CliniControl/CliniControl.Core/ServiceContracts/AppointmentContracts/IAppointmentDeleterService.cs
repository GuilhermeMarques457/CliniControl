using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentDeleterService
    {
        Task<bool> DeleteAppointment(Guid? appointmentID);
        Task<bool> DeleteAttachment(string? urlPathImg, Guid? appointmentID, string? wwwrootPath);
    }
}
