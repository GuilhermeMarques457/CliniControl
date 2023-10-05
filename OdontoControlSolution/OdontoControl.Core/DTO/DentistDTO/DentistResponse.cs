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

namespace OdontoControl.Core.DTO.DentistDTO
{
    public class DentistResponse
    {
        public Guid ID { get; set; }
        public string? DentistName { get; set; }
        public Guid ManagerID { get; set; }
        [ForeignKey("ManagerID")]
        public ApplicationUser? Manager { get; set; }
        public string? PhoneNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(DentistResponse)) return false;

            DentistResponse dentist = (DentistResponse)obj;

            return ID == dentist.ID &&
                DentistName == dentist.DentistName &&
                PhoneNumber == dentist.PhoneNumber &&
                ManagerID == dentist.ManagerID &&
                EndTime == dentist.EndTime &&
                StartTime == dentist.StartTime;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public DentistUpdateRequest ToDentistUpdateRequest()
        {
            return new DentistUpdateRequest
            {
                ID = ID,
                PhoneNumber = PhoneNumber,
                ManagerID = ManagerID,
                DentistName = DentistName,
                StartTime = StartTime,
                EndTime = EndTime,
            };
        }

        public Dentist ToDentist()
        {
            return new Dentist()
            {
                ID = ID,
                DentistName = DentistName,
                ManagerID = ManagerID,
                PhoneNumber = PhoneNumber,
                StartTime = StartTime,
                EndTime = EndTime,
            };
        }
    }

    public static class DentistExtensions
    {
        public static DentistResponse ToDentistResponse(this Dentist dentist)
        {
            return new DentistResponse
            {
                DentistName = dentist.DentistName,
                PhoneNumber = dentist.PhoneNumber,
                ID = dentist.ID,
                ManagerID = dentist.ManagerID,
                Manager = dentist.Manager,
                StartTime = dentist.StartTime,
                EndTime = dentist.EndTime,
            };
        }
    }
}
