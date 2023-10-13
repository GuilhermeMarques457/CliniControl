using OdontoControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdontoControl.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Mvc;

namespace OdontoControl.Core.DTO.DentistDTO
{
    public class DentistAddRequest
    {
        [Required(ErrorMessage = "Por favor o nome do dentista é obrigatório")]
        public string? DentistName { get; set; }

        [Required(ErrorMessage = "Por favor o nome do gerenciador é obrigatório")]
        public Guid ManagerID { get; set; }
        [ForeignKey("ManagerID")]
        public ApplicationUser? Manager { get; set; }

        [Required(ErrorMessage = "Por favor o telefone do dentista é obrigatório")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Por favor a hora de entrada do dentista é obrigatório")]
        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [Required(ErrorMessage = "Por favor a hora de saída do dentista é obrigatório")]
        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        public string? PhotoPath { get; set; }

        public Dentist ToDentist()
        {
            return new Dentist()
            {
                DentistName = DentistName,
                ManagerID = ManagerID,
                PhoneNumber = PhoneNumber,
                StartTime = StartTime,
                EndTime = EndTime,
                PhotoPath = PhotoPath
            };
        }
    }
}
