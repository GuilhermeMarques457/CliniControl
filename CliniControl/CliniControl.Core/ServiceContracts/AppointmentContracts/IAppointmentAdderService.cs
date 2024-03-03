using CliniControl.Core.DTO.AppointmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentAdderService
    {
        Task<AppointmentResponse> AddAppointment(AppointmentAddRequest appointment);
    }
}
