using OdontoControl.Core.DTO.AppointmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentAdderService
    {
        Task<AppointmentResponse> AddAppointment(AppointmentAddRequest appointment);
    }
}
