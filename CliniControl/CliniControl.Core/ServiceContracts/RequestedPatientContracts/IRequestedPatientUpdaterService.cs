using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.DTO.RequestedPatientDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.RequestedPatientContracts
{
    public interface IRequestedPatientUpdaterService
    {
        Task<RequestedPatientResponse> UpdateContactedStatusPatient(RequestedPatientUpdateRequest? Patient);
    }
}
