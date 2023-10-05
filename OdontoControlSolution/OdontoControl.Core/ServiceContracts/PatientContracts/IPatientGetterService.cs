using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.PatientDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.PatientContracts
{
    public interface IPatientGetterService
    {
        Task<PatientResponse?> GetPatientById(Guid? PatientID);
        Task<List<PatientResponse>> GetAllPatients();
        Task<List<PatientResponse>?> GetFilterdPatients(string? searchBy, string? searchString);
        Task<List<PatientResponse>?> GetPatientsByManagerID(Guid? ManagerID);
    }
}
