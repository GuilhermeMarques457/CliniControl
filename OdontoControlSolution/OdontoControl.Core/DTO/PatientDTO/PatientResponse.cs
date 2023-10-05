using OdontoControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.Enums;

namespace OdontoControl.Core.DTO.PatientDTO
{
    public class PatientResponse
    {
        public Guid ID { get; set; }

        public string? PatientName { get; set; }

        public Guid ManagerID { get; set; }

        [ForeignKey("ManagerID")]
        public ApplicationUser? Manager { get; set; }

        public string? CPF { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public string? PhotoPath { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PatientResponse)) return false;

            PatientResponse Patient = (PatientResponse)obj;

            return ID == Patient.ID &&
                PatientName == Patient.PatientName &&
                PhoneNumber == Patient.PhoneNumber &&
                ManagerID == Patient.ManagerID &&
                CPF == Patient.CPF &&
                Gender == Patient.Gender &&
                PhotoPath == Patient.PhotoPath;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public PatientUpdateRequest ToPatientUpdateRequest()
        {
            return new PatientUpdateRequest
            {
                ID = ID,
                PhoneNumber = PhoneNumber,
                ManagerID = ManagerID,
                PatientName = PatientName,
                CPF = CPF,
                PhotoPath = PhotoPath,
                Gender = Enum.TryParse(Gender, true, out GenderOptions gender) ? gender : GenderOptions.Indefinido,
            };
        }

        public Patient ToPatient()
        {
            return new Patient()
            {
                ID = ID,
                PatientName = PatientName,
                ManagerID = ManagerID,
                PhoneNumber = PhoneNumber,
                CPF = CPF,
                Gender = Gender,
                PhotoPath = PhotoPath,
            };
        }
    }

    public static class PatientExtensions
    {
        public static PatientResponse ToPatientResponse(this Patient Patient)
        {
            return new PatientResponse
            {
                PatientName = Patient.PatientName,
                PhoneNumber = Patient.PhoneNumber,
                ID = Patient.ID,
                ManagerID = Patient.ManagerID,
                Manager = Patient.Manager,
                CPF = Patient.CPF,
                Gender = Patient.Gender,
                PhotoPath = Patient.PhotoPath,
            };
        }
    }
}
