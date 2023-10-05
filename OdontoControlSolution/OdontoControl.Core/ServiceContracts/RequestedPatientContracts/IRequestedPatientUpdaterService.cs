using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.DTO.RequestedPatientDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.RequestedPatientContracts
{
    public interface IRequestedPatientUpdaterService
    {
        Task<RequestedPatientResponse> UpdateContactedStatusPatient(RequestedPatientUpdateRequest? Patient);
    }
}
