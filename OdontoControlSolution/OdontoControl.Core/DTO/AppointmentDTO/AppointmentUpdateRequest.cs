using OdontoControl.Core.CustomValidators;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.DTO.AppointmentDTO
{
    public class AppointmentUpdateRequest
    {
        [Required]
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Por favor selecione paciente válido")]
        public Guid? PatientID { get; set; }
        [Required(ErrorMessage = "Por favor selecione dentista válido")]
        public Guid? DentistID { get; set; }

        [Required(ErrorMessage = "Por favor selecione um procedimento válido")]
        public ProcedureTypeOptions? ProcedureType { get; set; }
        public AppointmentStatusOptions? Status { get; set; }
        public string? Comments { get; set; }

        [Required(ErrorMessage = "Por favor selecione um dia válido")]
        [DataType(DataType.Date)]
        [DateValidatorAttibute(ErrorMessage = "Data deve ser maior que o dia de hoje")]
        public DateTime? AppointmentTime { get; set; }

        [Required(ErrorMessage = "Por favor selecione um horário inicial válido")]
        [DataType(DataType.Time)]
        [TimeSpanValidator(ErrorMessage = "Valor deve ser um horário válido")]
        public TimeSpan? StartTime { get; set; }

        [Required(ErrorMessage = "Por favor selecione um horário final válido")]
        [DataType(DataType.Time)]
        [TimeSpanValidator(ErrorMessage = "Valor deve ser um horário válido")]
        public TimeSpan? EndTime { get; set; }
        public bool? Reminded { get; set; }
        public string? ExamsPath { get; set; }


        public Appointment ToAppointment()
        {
            return new Appointment()
            {
                ID = ID,
                PatientID = PatientID,
                DentistID = DentistID,
                ProcedureType = ProcedureType.ToString(),
                Status = Status.ToString(),
                Comments = Comments,
                AppointmentTime = AppointmentTime,
                StartTime = StartTime.ToString(),
                EndTime = EndTime.ToString(),
                Reminded = Reminded,
                ExamsPath = ExamsPath,
            };
        }
    }
}
