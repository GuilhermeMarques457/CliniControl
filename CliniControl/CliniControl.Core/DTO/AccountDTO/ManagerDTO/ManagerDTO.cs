using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliniControl.Core.Domain.IdentityEntities;

namespace CliniControl.Core.DTO.AccountDTO.ManagerDTO
{
    public class ManagerDTO : BaseDTO
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Cliníca não pode ser nulo")]
        public Guid? ClinicID { get; set; }

        [ForeignKey("ClinicID")]
        public Clinic? Clinic { get; set; }

    }

    public static class ManagerDTOExtensions
    {
        public static ManagerDTO ToManagerDTO(this ApplicationUser user)
        {
            return new ManagerDTO()
            {
                ID = user.Id,
                Email = user.Email,
                ClinicID = user.ClinicID,
                Phone = user.PhoneNumber,
                PersonName = user.PersonName,
            };
        }

        public static ManagerDTO ToManagerDTO(this RegisterManagerDTO user)
        {
            return new ManagerDTO()
            {
                ID = user.ID,
                Email = user.Email,
                ClinicID = user.ClinicID,
                Phone = user.Phone,
                PersonName = user.PersonName,
            };
        }
    }

}
