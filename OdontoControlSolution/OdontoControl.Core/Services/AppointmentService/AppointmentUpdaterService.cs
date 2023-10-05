using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.Helpers;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace OdontoControl.Core.Services.AppointmentService
{
    public class AppointmentUpdaterService : IAppointmentUpdaterService
    {
        private readonly IAppointmentRepository _repository;


        public AppointmentUpdaterService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<AppointmentResponse?> AddExamToAppointment(Guid? appointmentID, IFormFile? filePath, string? imagePathFolder)
        {
            Appointment? appointment = await _repository.GetAppointmentById(appointmentID);

            if (appointment != null)
            {
                List<string>? examsPathList = !string.IsNullOrEmpty(appointment.ExamsPath)
                    ? JsonSerializer.Deserialize<List<string>>(appointment.ExamsPath)
                    : new List<string>();

                if (filePath != null && imagePathFolder != null)
                {
                    await ManageImageProject.AddImage(filePath, imagePathFolder, "imgs/exams", appointmentID);

                    examsPathList?.Add($"/imgs/exams/{appointmentID}-{filePath.FileName}");
                }
                else
                    return null;

                appointment.ExamsPath = JsonSerializer.Serialize(examsPathList);

                Appointment? appointmentUpdated = await _repository.AddExamToAppointment(appointment);

                return appointmentUpdated?.ToAppointmentResponse();
            }
            else
            {
                return null;
            }
        }

        public async Task<AppointmentResponse> UpdateAppointment(AppointmentUpdateRequest appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            Appointment? existingAppointment = await _repository.GetAppointmentById(appointment.ID);

            if(existingAppointment == null)
                throw new ArgumentException(nameof(existingAppointment));

            Appointment? updatedAppointment = await _repository.UpdateAppointment(appointment.ToAppointment()); 

            if (updatedAppointment == null)
                throw new ArgumentException(nameof(updatedAppointment));

            return updatedAppointment.ToAppointmentResponse();
        }
    }
}
