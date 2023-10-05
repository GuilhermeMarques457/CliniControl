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

namespace OdontoControl.Core.DTO.RequestedPatientDTO
{
    public class RequestedPatientResponse
    {
        [Key] public Guid ID { get; set; }
        public string? PatientName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Contacted { get; set; }
        public Guid? ClinicID { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(RequestedPatientResponse)) return false;

            RequestedPatientResponse Patient = (RequestedPatientResponse)obj;

            return ID == Patient.ID &&
                PatientName == Patient.PatientName &&
                PhoneNumber == Patient.PhoneNumber &&
                Contacted == Patient.Contacted &&
                ClinicID == Patient.ClinicID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public RequestedPatientUpdateRequest ToPatientUpdateRequest()
        {
            return new RequestedPatientUpdateRequest
            {
                ID = ID,
                Contacted = Contacted,
            };
        }

        public RequestedPatient ToPatient()
        {
            return new RequestedPatient()
            {
                ID = ID,
                PatientName = PatientName,
                PhoneNumber = PhoneNumber,
                Contacted = Contacted,
                ClinicID = ClinicID,
                
            };
        }
    }

    public static class RequestedPatientExtensions
    {
        public static RequestedPatientResponse ToRequestedPatientResponse(this RequestedPatient Patient)
        {
            return new RequestedPatientResponse
            {
                PatientName = Patient.PatientName,
                PhoneNumber = Patient.PhoneNumber,
                ID = Patient.ID,
                Contacted = Patient.Contacted,
                ClinicID = Patient.ClinicID,
            };
        }
    }
}
