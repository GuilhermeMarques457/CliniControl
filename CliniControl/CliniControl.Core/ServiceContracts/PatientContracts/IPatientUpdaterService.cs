using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.PatientDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.PatientContracts
{
    public interface IPatientUpdaterService
    {
        Task<PatientResponse> UpdatePatient(PatientUpdateRequest Patient);
    }
}
