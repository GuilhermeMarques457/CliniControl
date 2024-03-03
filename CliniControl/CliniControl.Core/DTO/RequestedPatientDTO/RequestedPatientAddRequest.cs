using CliniControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliniControl.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Enums;

namespace CliniControl.Core.DTO.RequestedPatientDTO
{
    public class RequestedPatientAddRequest
    {
        [Required(ErrorMessage = "Por favor o seu nome é obrigatório")]
        public string? PatientName { get; set; }
      
        [Required(ErrorMessage = "Por favor informe o seu numero de telefone para contato")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Este campo deve conter apenas números.")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        public Guid? ClinicID { get; set; }

        public RequestedPatient ToPatient()
        {
            return new RequestedPatient()
            {
                ClinicID = ClinicID,
                PatientName = PatientName,
                PhoneNumber = PhoneNumber
            };
        }
    }
}
