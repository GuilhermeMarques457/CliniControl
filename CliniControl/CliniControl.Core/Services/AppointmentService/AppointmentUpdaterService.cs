using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.Helpers;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CliniControl.Core.Enums;

namespace CliniControl.Core.Services.AppointmentService
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

        public async Task<AppointmentResponse> UpdateAppointment(AppointmentUpdateRequest? appointment)
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

        public async Task<List<AppointmentResponse>?> UpdateAppointmentStatus(List<AppointmentUpdateRequest>? appointmentList)
        {
            if (appointmentList == null)
                return null;

            List<Appointment>? appointmentUpdatedList = new List<Appointment>();

            foreach (var appointment in appointmentList)
            {
                DateTime? dateStartTheAppoitment = appointment.AppointmentTime;

                if (appointment.StartTime != null)
                    dateStartTheAppoitment = dateStartTheAppoitment!.Value.Add((TimeSpan)appointment.StartTime);

                DateTime? dateEndTheAppoitment = appointment.AppointmentTime;

                if (appointment.EndTime != null)
                    dateEndTheAppoitment = dateEndTheAppoitment!.Value.Add((TimeSpan)appointment.EndTime);

                Appointment? appointmentUpdated = null;

                if (dateStartTheAppoitment < DateTime.Now && dateEndTheAppoitment > DateTime.Now && appointment.Status != AppointmentStatusOptions.Atendimento)
                {
                    appointment.Status = AppointmentStatusOptions.Atendimento;

                    appointmentUpdated = await _repository.UpdateAppointmentStatus(appointment.ToAppointment());
                }
                else if (dateEndTheAppoitment < DateTime.Now && appointment.Status != AppointmentStatusOptions.Receber && appointment.Status != AppointmentStatusOptions.Pago)
                {
                    appointment.Status = AppointmentStatusOptions.Receber;

                    appointmentUpdated = await _repository.UpdateAppointmentStatus(appointment.ToAppointment());
                }

                if(appointmentUpdated != null)
                {
                    appointmentUpdatedList!.Add(appointmentUpdated);
                } 
            }

            if (appointmentUpdatedList == null)
                return null;

            return appointmentUpdatedList.Select(temp => temp.ToAppointmentResponse()).ToList();
        }
    }
}
