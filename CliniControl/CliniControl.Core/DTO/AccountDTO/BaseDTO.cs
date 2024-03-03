using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.DTO.AccountDTO
{
    public abstract class BaseDTO
    {
        [Required(ErrorMessage = "Email não pode ser nulo")]
        [EmailAddress(ErrorMessage = "Utilize um formato de email apropriado")]
        [DataType(DataType.EmailAddress)]
        public virtual string? Email { get; set; }

        [Required(ErrorMessage = "O nome não pode ser nulo")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Senha não pode ser nula")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirmar senha não pode ser nula")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Senha e confirmar senha não são iguais")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Numero de telefone não pode ser nulo")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numero de telefone deve conter apenas números")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public virtual UserTypeOptions UserType { get; set; }
    }
}
