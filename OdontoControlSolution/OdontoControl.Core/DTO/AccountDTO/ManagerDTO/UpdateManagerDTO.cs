using Microsoft.AspNetCore.Mvc;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.DTO.AccountDTO.ManagerDTO
{
    public class UpdateManagerDTO
    {
        [Required(ErrorMessage = "Email não pode ser nulo")]
        [EmailAddress(ErrorMessage = "Utilize um formato de email apropriado")]
        [DataType(DataType.EmailAddress)]
        public virtual string? Email { get; set; }

        [Required(ErrorMessage = "O nome não pode ser nulo")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Numero de telefone não pode ser nulo")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numero de telefone deve conter apenas números")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public virtual UserTypeOptions UserType { get; set; }

        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Por favor selecione uma Cliníca Válida")]
        public Guid? ClinicID { get; set; }

        [ForeignKey("ClinicID")]
        public Clinic? Clinic { get; set; }
    }

    public static class UpdateManagerDTOExtensions
    {
        public static UpdateManagerDTO ToUpdateManagerDTO(this ApplicationUser user)
        {
            return new UpdateManagerDTO()
            {
                ID = user.Id,
                Email = user.Email,
                ClinicID = user.ClinicID,
                Phone = user.PhoneNumber,
                PersonName = user.PersonName,
            };
        }
    }
}
