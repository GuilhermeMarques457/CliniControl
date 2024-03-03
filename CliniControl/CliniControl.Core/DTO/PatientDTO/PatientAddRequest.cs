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

namespace CliniControl.Core.DTO.PatientDTO
{
    public class PatientAddRequest
    {
        [Required(ErrorMessage = "Por favor o nome do Paciente é obrigatório")]
        public string? PatientName { get; set; }

        [Required(ErrorMessage = "Por favor o ID do gerenciador é obrigatório")]
        public Guid ManagerID { get; set; }
        [ForeignKey("ManagerID")]
        public ApplicationUser? Manager { get; set; }

        [Required(ErrorMessage = "Por favor o CPF do Paciente é obrigatório")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Por favor insira um CPF válido")]
        public string? CPF { get; set; }

        [Required(ErrorMessage = "Por favor informe o telefone do Paciente")]
        [RegularExpression(@"^\(\d{2}\) \d{5}-\d{4}$", ErrorMessage = "O número de telefone deve estar no formato (XX) XXXXX-XXXX")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Por favor informe o gênero do Paciente")]
        public GenderOptions? Gender { get; set; }

        public string? PhotoPath { get; set; }

        public Patient ToPatient()
        {
            return new Patient()
            {
                PatientName = PatientName,
                ManagerID = ManagerID,
                PhoneNumber = PhoneNumber,
                CPF = CPF,
                Gender = Gender.ToString(),
                PhotoPath = PhotoPath

            };
        }
    }
}
