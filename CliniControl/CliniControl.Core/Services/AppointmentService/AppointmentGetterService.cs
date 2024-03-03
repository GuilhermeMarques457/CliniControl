using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace CliniControl.Core.Services.AppointmentService
{
    public class AppointmentGetterService : IAppointmentGetterService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentGetterService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AppointmentResponse>> GetAllAppointments()
        {
            List<Appointment> appointments = await _repository.GetAllAppointments();

            return appointments.Select(temp => temp.ToAppointmentResponse()).ToList();
        }

        public async Task<List<AppointmentResponse>?> GetAllDayAppointments(DateTime? appointmentDay, Guid? DentistID)
        {
            if(appointmentDay == null || DentistID == null)
            {
                return null;
            }

            List<Appointment>? appointmentsOfTheDay = await _repository.GetAllDayAppointments(appointmentDay, DentistID);

            return appointmentsOfTheDay.Select(temp => temp.ToAppointmentResponse()).ToList();
        }

        public async Task<AppointmentResponse?> GetAppointmentById(Guid? appointmentID)
        {
            if (appointmentID == null)
                return null;

            Appointment? appointment = await _repository.GetAppointmentById(appointmentID);

            if (appointment == null)
                return null;

            return appointment.ToAppointmentResponse();
        }

        public async Task<List<AppointmentResponse>?> GetAppointmentByPatientId(Guid? patientID)
        {
            if (patientID == null)
                return null;

            List<Appointment>? appointmentList = await _repository.GetAppointmentByPatientId(patientID);

            if (appointmentList == null)
                return null;

            return appointmentList.Select(temp => temp.ToAppointmentResponse()).ToList();
        }

        public async Task<List<AppointmentResponse>?> GetFilterdAppointments(string? searchBy, string? searchString)
        {
            List<Appointment>? Appointments = new List<Appointment>();

            if (searchString == null)
            {
                Appointments = await _repository.GetAllAppointments();
            }
            else
            {
                Appointments = searchBy switch
                {
                    nameof(AppointmentResponse.Patient.PatientName) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.Patient!.PatientName!.Contains(searchString)),
                    nameof(AppointmentResponse.Dentist.DentistName) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.Dentist!.DentistName!.Contains(searchString.ToString())),
                    nameof(AppointmentResponse.Status) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.Status!.Contains(searchString)),
                    nameof(AppointmentResponse.AppointmentTime) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.AppointmentTime!.ToString()!.Contains(searchString)),
                    nameof(AppointmentResponse.StartTime) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.StartTime!.Contains(searchString)),
                    nameof(AppointmentResponse.EndTime) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.EndTime!.Contains(searchString)),
                    nameof(AppointmentResponse.ProcedureType) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.ProcedureType!.Contains(searchString)),
                    nameof(AppointmentResponse.Price) =>
                         await _repository.GetFilteredAppointments(temp =>
                            temp.Price!.ToString()!.Contains(searchString)),
                    nameof(AppointmentResponse.Patient.PhoneNumber) =>
                       await _repository.GetFilteredAppointments(temp =>
                           temp.Patient!.PhoneNumber!.Contains(searchString)),
                    _ => await _repository.GetAllAppointments()
                };
            }

            return Appointments.Select(temp => temp.ToAppointmentResponse()).ToList();
        }

        public async Task<List<AppointmentResponse>?> GetFilterdDayAppointments(string? searchBy, string? searchString, DateTime? appointmentsDay)
        {
            List<Appointment>? Appointments = new List<Appointment>();

            if (searchString == null)
            {
                Appointments = await _repository.GetAllAppointments();
            }
            else
            {
                Appointments = searchBy switch
                {
                    nameof(AppointmentResponse.Patient.PatientName) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.Patient!.PatientName!.Contains(searchString) && temp.AppointmentTime == appointmentsDay),
                    nameof(AppointmentResponse.Dentist.DentistName) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.Dentist!.DentistName!.Contains(searchString.ToString()) && temp.AppointmentTime == appointmentsDay),
                    nameof(AppointmentResponse.Status) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.Status!.Contains(searchString) && temp.AppointmentTime == appointmentsDay),
                    nameof(AppointmentResponse.AppointmentTime) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.AppointmentTime.ToString()!.Contains(searchString) && temp.AppointmentTime == appointmentsDay),
                    nameof(AppointmentResponse.StartTime) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.StartTime!.Contains(searchString) && temp.AppointmentTime == appointmentsDay),
                    nameof(AppointmentResponse.EndTime) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.EndTime!.Contains(searchString) && temp.AppointmentTime == appointmentsDay),
                    nameof(AppointmentResponse.ProcedureType) =>
                        await _repository.GetFilteredAppointments(temp =>
                            temp.ProcedureType!.Contains(searchString) && temp.AppointmentTime == appointmentsDay),
                    nameof(AppointmentResponse.Patient.PhoneNumber) =>
                       await _repository.GetFilteredAppointments(temp =>
                           temp.Patient!.PhoneNumber!.Contains(searchString) && temp.AppointmentTime == appointmentsDay),
                    _ => await _repository.GetAllAppointments()
                };
            }

            return Appointments.Select(temp => temp.ToAppointmentResponse()).ToList();
        }

        public async Task<List<AppointmentResponse>?> GetDayAppointments(DateTime? today)
        {
            if (today == null)
                return null;

            List<Appointment>? appointments = await _repository.GetDayAppointments(today);

            if (appointments.Count == 0)
                return null;

            return appointments.Select(temp => temp.ToAppointmentResponse()).ToList();
        }

        public async Task<List<AppointmentResponse>?> GetAppointmentsByPossibleStatusChange(DateTime? day)
        {
            List<Appointment>? appointments = await _repository.GetAppointmentsByPossibleStatusChange(day);

            if (appointments == null)
                return null;

            return appointments.Select(temp => temp.ToAppointmentResponse()).ToList();
        }

        public async Task<List<AppointmentResponse>?> GetAppointmentsByDentistId(Guid? dentistID)
        {
            if (dentistID == null)
                return null;

            List<Appointment>? appointmentList = await _repository.GetAppointmentsByDentistId(dentistID);

            if (appointmentList == null)
                return null;

            return appointmentList.Select(temp => temp.ToAppointmentResponse()).ToList();
        }
    }
}
