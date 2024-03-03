using CliniControl.Core.Domain.Entities;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CliniControl.Core.DTO.ClinicDTO
{
    public class ClinicResponse
    {
        public Guid ID { get; set; }
        public string? ClinicName { get; set; }
        public string? StreetName { get; set; }
        public string? Neighborhood { get; set; }
        public string? CNPJ { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(ClinicResponse)) return false;

            ClinicResponse clinic = (ClinicResponse) obj;
            return clinic.ID == ID &&
                clinic.ClinicName == ClinicName &&
                clinic.StreetName == StreetName &&
                clinic.City == City &&
                clinic.Neighborhood == Neighborhood &&
                clinic.CNPJ == CNPJ &&
                clinic.Phone == Phone;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public ClinicUpdateRequest ToClinicUpdateRequest()
        {
            return new ClinicUpdateRequest()
            {
                ID = ID,
                ClinicName = ClinicName,
                StreetName = StreetName,
                City = Enum.TryParse(City, true, out CitiesOptions city) ? city : CitiesOptions.Adamantina,
                Neighborhood = Neighborhood,
                CNPJ = CNPJ,
                Phone = Phone
            };
        }

        public Clinic ToClinic()
        {
            return new Clinic()
            {
                ID = ID,
                ClinicName = ClinicName,
                StreetName = StreetName,
                Neighborhood = Neighborhood,
                CNPJ = CNPJ,
                City = City,
                Phone = Phone
            };
        }
    }

    public static class ClinicExtensions
    {
        public static ClinicResponse ToClinicResponse(this Clinic clinic)
        {
            return new ClinicResponse
            {
                City = clinic.City,
                ClinicName = clinic.ClinicName,
                StreetName = clinic.StreetName,
                CNPJ = clinic.CNPJ,
                ID = clinic.ID,
                Neighborhood = clinic.Neighborhood,
                Phone = clinic.Phone
            };
        }
    }
}
