using OdontoControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.Enums;

namespace OdontoControl.Core.DTO.RequestedPatientDTO
{
    public class RequestedPatientUpdateRequest
    {
        [Required]
        public Guid ID { get; set; }

        public bool? Contacted { get; set; }

        public RequestedPatient ToPatient()
        {
            return new RequestedPatient()
            {
                ID = ID,
                Contacted = Contacted,
            };
        }
    }
}
