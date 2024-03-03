using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CliniControl.Core.DTO.AppointmentDTO
{
    public class AppointmentResponse
    {
        public Guid ID { get; set; }
        public Guid? DentistID { get; set; }

        [ForeignKey("DentistID")]
        public Dentist? Dentist { get; set; }
        public Guid? PatientID { get; set; }

        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }
        public string? ProcedureType { get; set; }
        public string? Status { get; set; }
        public string? Comments { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? ExamsPath { get; set; }
        public double? Price { get; set; }
        public List<string>? ExamsPathList { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != typeof(AppointmentResponse)) return false;

            AppointmentResponse appointment = (AppointmentResponse)obj;
            return ID == appointment.ID
                && PatientID == appointment.PatientID
                && DentistID == appointment.DentistID
                && ProcedureType == appointment.ProcedureType
                && Status == appointment.Status
                && StartTime == appointment.StartTime
                && EndTime == appointment.EndTime
                && AppointmentTime == appointment.AppointmentTime
                && Comments == appointment.Comments
                && ExamsPath == appointment.ExamsPath
                && Price == appointment.Price
                && ExamsPathList == appointment.ExamsPathList;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public AppointmentUpdateRequest ToAppointmentUpdateRequest()
        {
            return new AppointmentUpdateRequest
            {
                ID = ID,
                PatientID = PatientID,
                DentistID = DentistID,
                ProcedureType = Enum.TryParse(ProcedureType, true, out ProcedureTypeOptions procedureType) ? procedureType : ProcedureTypeOptions.Agendado,
                Comments = Comments,
                AppointmentTime = AppointmentTime,
                Status = Enum.TryParse(Status, true, out AppointmentStatusOptions status) ? status : AppointmentStatusOptions.Agendado,
                StartTime = StartTime,
                EndTime = EndTime,
                ExamsPath = ExamsPath,
                Price = Price,
            };
        }
    }

    public static class AppointmentExtensions
    {
        public static AppointmentResponse ToAppointmentResponse(this Appointment appointment)
        {
            return new AppointmentResponse
            {
                ID = appointment.ID,
                PatientID = appointment.PatientID,
                DentistID = appointment.DentistID,
                ProcedureType = appointment.ProcedureType,
                Comments = appointment.Comments,
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status,
                StartTime = TimeSpan.Parse(appointment.StartTime!.ToString()),
                EndTime = TimeSpan.Parse(appointment.EndTime!.ToString()),
                Patient = appointment.Patient,
                Dentist = appointment.Dentist,
                Price = appointment.Price,
                ExamsPath = appointment.ExamsPath,
                ExamsPathList = !string.IsNullOrEmpty(appointment.ExamsPath)
                    ? JsonSerializer.Deserialize<List<string>>(appointment.ExamsPath)
                    : new List<string>()
        };
        }
    }
}
