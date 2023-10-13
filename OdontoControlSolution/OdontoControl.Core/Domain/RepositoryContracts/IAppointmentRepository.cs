using OdontoControl.Core.Domain.Entities;
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
        Task<bool> DeleteAppointment(Guid? appointmentID);
        Task<bool> DeleteAttachment(string urlPathImg, Guid appointmentID);
        Task<Appointment?> GetAppointmentById(Guid? AppointmentID);
        Task<List<Appointment>?> GetAppointmentByPatientId(Guid? patientID);
        Task<List<Appointment>> GetAllAppointments();
        Task<List<Appointment>> GetFilteredAppointments(Expression<Func<Appointment, bool>> predicate);
        Task<List<Appointment>> GetAllDayAppointments(DateTime? DayOfAppointment, Guid? DentistID);
        Task<List<Appointment>> GetDayAppointments(DateTime? today);
        Task<Appointment?> AddExamToAppointment(Appointment appointment);

    }
}
