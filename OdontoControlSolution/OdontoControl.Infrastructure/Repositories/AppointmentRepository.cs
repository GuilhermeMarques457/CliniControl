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
using OdontoControl.Core.Helpers;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
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
            Appointment? appointment = await _context.Appointments.FirstOrDefaultAsync(temp => temp.ID == appointmentID);

            if (appointment == null)
                return false;

            _context.Appointments.Remove(appointment);

            int rowsAfected = await _context.SaveChangesAsync();

            return rowsAfected > 0;
        }

        public async Task<bool> DeleteAttachment(string urlPathImg, Guid appointmentID)
        {
            Appointment? appointment = await _context.Appointments.FirstOrDefaultAsync(temp => temp.ID == appointmentID);

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

        public async Task<Appointment?> UpdateAppointment(Appointment appointment)
        {
            Appointment? matchingAppointment = await _context.Appointments.Include("Patient").Include("Dentist").FirstOrDefaultAsync(temp => temp.ID == appointment.ID);

            if (matchingAppointment != null) 
            {
                matchingAppointment.AppointmentTime = appointment.AppointmentTime;
                matchingAppointment.Comments = appointment.Comments;
                matchingAppointment.PatientID = appointment.PatientID;
                matchingAppointment.DentistID = appointment.DentistID;
                matchingAppointment.ProcedureType = appointment.ProcedureType;
                matchingAppointment.StartTime = appointment.StartTime;
                matchingAppointment.EndTime = appointment.EndTime;
                matchingAppointment.Status = appointment.Status;
                matchingAppointment.ExamsPath = appointment.ExamsPath;
            }

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
            Appointment? matchingAppointment = await _context.Appointments.Include("Patient").Include("Dentist").FirstOrDefaultAsync(temp => temp.ID == appointment.ID);

            if (matchingAppointment != null)
                matchingAppointment.ExamsPath = appointment.ExamsPath;

            await _context.SaveChangesAsync();

            return matchingAppointment;
        }
    }
}
