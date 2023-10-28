using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.Enums;
using OdontoControl.Core.Helpers;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.Services.AppointmentService;
using OdontoControl.Infrastructure.DbContext;

namespace OdontoControl.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> AddAppointment(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task<bool> DeleteAppointment(Guid? appointmentID)
        {
            Appointment? appointment = await GetAppointmentById(appointmentID);

            if (appointment == null)
                return false;

            _context.Appointments.Remove(appointment);

            int rowsAfected = await _context.SaveChangesAsync();

            return rowsAfected > 0;
        }

        public async Task<bool> DeleteAttachment(string urlPathImg, Guid appointmentID)
        {
            Appointment? appointment = await GetAppointmentById(appointmentID);

            if (appointment == null)
                return false;

            List<string>? examsPathList = !string.IsNullOrEmpty(appointment.ExamsPath)
                    ? JsonSerializer.Deserialize<List<string>>(appointment.ExamsPath)
                    : new List<string>();

            if(examsPathList!.Count == 0) return false;

            examsPathList.Remove(urlPathImg.Replace("~", ""));

            appointment.ExamsPath = JsonSerializer.Serialize(examsPathList);

            await UpdateAppointment(appointment);

            int rowsAfected = await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _context.Appointments.Include("Patient").Include("Dentist").ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentById(Guid? AppointmentID)
        {
            return await _context.Appointments.Include("Patient").Include("Dentist").FirstOrDefaultAsync(temp => temp.ID == AppointmentID);
        }

        public async Task<List<Appointment>?> GetAppointmentByPatientId(Guid? patientID)
        {
            return await _context.Appointments.Include("Patient").Include("Dentist").Where(temp => temp.PatientID == patientID).ToListAsync();
        }

        public async Task<Appointment?> UpdateAppointment(Appointment appointment)
        {
            _context.ChangeTracker.Clear();

            Appointment? matchingAppointment = await GetAppointmentById(appointment.ID);

            if (matchingAppointment == null) return null;
            
            matchingAppointment.AppointmentTime = appointment.AppointmentTime;
            matchingAppointment.Comments = appointment.Comments;
            matchingAppointment.PatientID = appointment.PatientID;
            matchingAppointment.DentistID = appointment.DentistID;
            matchingAppointment.ProcedureType = appointment.ProcedureType;
            matchingAppointment.StartTime = appointment.StartTime;
            matchingAppointment.EndTime = appointment.EndTime;
            matchingAppointment.Status = appointment.Status;
            matchingAppointment.ExamsPath = appointment.ExamsPath;
            matchingAppointment.Price = appointment.Price;

            _context.Appointments.Update(matchingAppointment);

            await _context.SaveChangesAsync();

            return matchingAppointment;
        }

        public async Task<List<Appointment>> GetFilteredAppointments(Expression<Func<Appointment, bool>> predicate)
        {
            return await _context.Appointments.Include("Patient").Include("Dentist").Where(predicate).ToListAsync();
        }

        public async Task<List<Appointment>> GetAllDayAppointments(DateTime? DayOfAppointment, Guid? DentistID)
        {
            return await _context.Appointments.Include("Patient").Include("Dentist").Where(temp => temp.AppointmentTime == DayOfAppointment && temp.DentistID == DentistID).ToListAsync();
        }

        public async Task<List<Appointment>> GetDayAppointments(DateTime? today)
        {
            return await _context.Appointments.Include("Patient").Include("Dentist").Where(temp => temp.AppointmentTime == today).ToListAsync();
        }

        public async Task<Appointment?> AddExamToAppointment(Appointment appointment)
        {
            _context.ChangeTracker.Clear();

            Appointment? matchingAppointment = await GetAppointmentById(appointment.ID);

            if (matchingAppointment == null) return null;

            matchingAppointment.ExamsPath = appointment.ExamsPath;

            _context.Appointments.Update(matchingAppointment);

            await _context.SaveChangesAsync();

            return matchingAppointment;
        }

        public async Task<List<Appointment>?> GetAppointmentsByPossibleStatusChange(DateTime? day)
        {
            List<Appointment>? appointments = null;

            if (day == null)
            {
                appointments = await _context.Appointments
                    .Where(temp =>
                        temp.Status != AppointmentStatusOptions.Pago.ToString() &&
                        temp.Status != AppointmentStatusOptions.Receber.ToString()
                    ).ToListAsync();
            }
            else
            {
                appointments = await _context.Appointments
                    .Where(temp =>
                        temp.Status != AppointmentStatusOptions.Pago.ToString() &&
                        temp.Status != AppointmentStatusOptions.Receber.ToString() &&
                        temp.AppointmentTime!.Value == day.Value
                    ).ToListAsync();
            }

            return appointments;
        }

        public async Task<Appointment?> UpdateAppointmentStatus(Appointment appointment)
        {
            _context.ChangeTracker.Clear();

            Appointment? matchingAppointment = await GetAppointmentById(appointment.ID);

            if (matchingAppointment == null) return null;

            matchingAppointment.Status = appointment.Status;

            _context.Appointments.Update(matchingAppointment);

            await _context.SaveChangesAsync();

            return matchingAppointment;
        }

        public async Task<List<Appointment>?> GetAppointmentsByDentistId(Guid? dentistID)
        {
            return await _context.Appointments.Include("Patient").Include("Dentist").Where(temp => temp.DentistID == dentistID).ToListAsync();
        }
    }
}
