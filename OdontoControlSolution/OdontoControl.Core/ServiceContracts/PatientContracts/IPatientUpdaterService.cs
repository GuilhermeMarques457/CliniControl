using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.PatientDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.PatientContracts
{
    public interface IPatientUpdaterService
    {
        Task<PatientResponse> UpdatePatient(PatientUpdateRequest Patient);
    }
}
