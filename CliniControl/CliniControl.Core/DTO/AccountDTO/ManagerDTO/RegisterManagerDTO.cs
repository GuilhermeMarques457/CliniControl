using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.DTO.AccountDTO.ManagerDTO
{
    public class RegisterManagerDTO : BaseDTO
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Por favor selecione uma Cliníca Válida")]
        public Guid? ClinicID { get; set; }

        [ForeignKey("ClinicID")]
        public Clinic? Clinic { get; set; }
        public override UserTypeOptions UserType { get; set; } = UserTypeOptions.Manager;

        [Remote("EmailValidator", controller: "Account", ErrorMessage = "Email já esta em uso")]
        public override string? Email { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
