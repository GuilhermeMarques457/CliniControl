using CliniControl.Core.Domain.Entities;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.DentistDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentGetterService
    {
        Task<AppointmentResponse?> GetAppointmentById(Guid? appointmentID);
        Task<List<AppointmentResponse>?> GetAppointmentByPatientId(Guid? patientID);
        Task<List<AppointmentResponse>?> GetAppointmentsByDentistId(Guid? dentistID);
        Task<List<AppointmentResponse>> GetAllAppointments();
        Task<List<AppointmentResponse>?> GetFilterdAppointments(string? searchBy, string? searchString);
        Task<List<AppointmentResponse>?> GetFilterdDayAppointments(string? searchBy, string? searchString, DateTime? appointmentsDay);
        Task<List<AppointmentResponse>?> GetAllDayAppointments(DateTime? appointmentDay, Guid? DentistID);
        Task<List<AppointmentResponse>?> GetDayAppointments(DateTime? today);
        Task<List<AppointmentResponse>?> GetAppointmentsByPossibleStatusChange(DateTime? day);


    }
}
