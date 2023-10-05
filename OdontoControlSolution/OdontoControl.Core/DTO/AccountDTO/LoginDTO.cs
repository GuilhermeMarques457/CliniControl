using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.DTO.AccountDTO
{
    public class LoginDTO
    {
        [EmailAddress(ErrorMessage = "Utilize um formato de email apropriado")]
        [Required(ErrorMessage = "Email não pode ser nulo")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha não pode ser nulo")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
