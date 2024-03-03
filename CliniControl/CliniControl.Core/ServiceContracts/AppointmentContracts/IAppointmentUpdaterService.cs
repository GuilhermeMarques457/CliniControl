using Microsoft.AspNetCore.Http;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.DTO.AppointmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentUpdaterService
    {
        Task<AppointmentResponse> UpdateAppointment(AppointmentUpdateRequest? appointment);
        Task<AppointmentResponse?> AddExamToAppointment(Guid? appointmentID, IFormFile? formFile, string? imagePathFolder);
        Task<List<AppointmentResponse>?> UpdateAppointmentStatus(List<AppointmentUpdateRequest>? appointmentList);
    }
}
