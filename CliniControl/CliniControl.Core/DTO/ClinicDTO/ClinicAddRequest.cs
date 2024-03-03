using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.DTO.ClinicDTO
{
    public class ClinicAddRequest
    {

        [Required(ErrorMessage = "Por favor informe o nome da clinica")]
        public string? ClinicName { get; set; }

        [Required(ErrorMessage = "Por favor informe a rua da clinica")]
        public string? StreetName { get; set; }

        [Required(ErrorMessage = "Por favor informe o bairro da clinica")]
        public string? Neighborhood { get; set; }

        [Required(ErrorMessage = "Por favor informe o CNPJ da clinica")]
        [RegularExpression("^(?!(\\d)\\1{13})\\d{2}\\.\\d{3}\\.\\d{3}\\/\\d{4}\\-\\d{2}$", ErrorMessage = "Por favor insira um CNPJ válido")]
        public string? CNPJ { get; set; }

        [Required(ErrorMessage = "Por favorinforme o cidade da clinica")]
        public CitiesOptions? City { get; set; }

        [Required(ErrorMessage = "Por favor informe o telefone da clinica")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numero de telefone deve conter apenas números")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public Clinic ToClinic()
        {
            return new Clinic()
            {
                ClinicName = ClinicName,
                StreetName = StreetName,
                Neighborhood = Neighborhood,
                CNPJ = CNPJ,
                City = City.ToString(),
                Phone = Phone
            };
        }
    }
}
