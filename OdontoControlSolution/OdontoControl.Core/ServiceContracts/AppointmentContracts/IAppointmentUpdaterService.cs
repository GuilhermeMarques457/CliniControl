using Microsoft.AspNetCore.Http;
using OdontoControl.Core.DTO.AppointmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentUpdaterService
    {
        Task<AppointmentResponse> UpdateAppointment(AppointmentUpdateRequest appointment);
        Task<AppointmentResponse?> AddExamToAppointment(Guid? appointmentID, IFormFile? formFile, string? imagePathFolder);
    }
}
