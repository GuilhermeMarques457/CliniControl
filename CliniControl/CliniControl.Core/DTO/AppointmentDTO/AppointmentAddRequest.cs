﻿using CliniControl.Core.CustomValidators;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.DTO.AppointmentDTO
{
    public class AppointmentAddRequest
    {
        [Required(ErrorMessage = "Por favor selecione paciente válido")]
        public Guid? PatientID { get; set; }

        [Required(ErrorMessage = "Por favor selecione dentista válido")]
        public Guid? DentistID { get; set; }

        [Required(ErrorMessage = "Por favor selecione um procedimento válido")]
        public ProcedureTypeOptions? ProcedureType { get; set; }
        public AppointmentStatusOptions? Status { get; set; } = AppointmentStatusOptions.Agendado;
        public string? Comments { get; set; }

        [Required(ErrorMessage = "Por favor selecione um dia válido")]
        [DataType(DataType.Date)]
        [DateValidatorAttibute(ErrorMessage = "Data deve ser maior que o dia de hoje")]
        public DateTime? AppointmentTime { get; set; }

        [Required(ErrorMessage = "Por favor selecione um horário inicial válido")]
        [TimeSpanValidator(ErrorMessage = "Valor deve ser um horário válido")]
        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [Required(ErrorMessage = "Por favor selecione um horário final válido")]
        [TimeSpanValidator(ErrorMessage = "Valor deve ser um horário válido")]
        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        [Required(ErrorMessage = "Por favor o preço é obrigatório")]
        [RegularExpression("^\\d+$", ErrorMessage = "Apenas numeros são permitidos")]
        public double? Price { get; set; }

        public string? ExamsPath { get; set; }

        public Appointment ToAppointment()
        {
            return new Appointment()
            {
                PatientID = PatientID,
                DentistID = DentistID,
                StartTime = StartTime.ToString(),
                EndTime = EndTime.ToString(),
                ProcedureType = ProcedureType.ToString(),
                Status = Status.ToString(),
                Comments = Comments,
                AppointmentTime = AppointmentTime,
                ExamsPath = ExamsPath,
                Price = Price,
            };
        }
    }
}
