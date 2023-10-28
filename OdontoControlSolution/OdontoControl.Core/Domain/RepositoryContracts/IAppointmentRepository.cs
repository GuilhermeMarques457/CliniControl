using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.DTO.AppointmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Domain.RepositoryContracts
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> AddAppointment(Appointment appointment);
        Task<Appointment?> UpdateAppointment(Appointment appointment);
        Task<Appointment?> UpdateAppointmentStatus(Appointment appointment);
        Task<bool> DeleteAppointment(Guid? appointmentID);
        Task<bool> DeleteAttachment(string urlPathImg, Guid appointmentID);
        Task<Appointment?> GetAppointmentById(Guid? AppointmentID);
        Task<List<Appointment>?> GetAppointmentByPatientId(Guid? patientID);
        Task<List<Appointment>?> GetAppointmentsByPossibleStatusChange(DateTime? day);
        Task<List<Appointment>> GetAllAppointments();
        Task<List<Appointment>> GetFilteredAppointments(Expression<Func<Appointment, bool>> predicate);
        Task<List<Appointment>> GetAllDayAppointments(DateTime? DayOfAppointment, Guid? DentistID);
        Task<List<Appointment>?> GetAppointmentsByDentistId(Guid? dentistID);
        Task<List<Appointment>> GetDayAppointments(DateTime? today);
        Task<Appointment?> AddExamToAppointment(Appointment appointment);

    }
}
