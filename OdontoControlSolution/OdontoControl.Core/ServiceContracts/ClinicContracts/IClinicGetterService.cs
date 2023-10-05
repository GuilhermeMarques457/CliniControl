using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.ClinicContracts
{
    public interface IClinicGetterService
    {
        Task<ClinicResponse?> GetClinicById(Guid? clinicID);
        Task<ClinicResponse?> GetClinicByName(string? clinicName);
        Task<List<ClinicResponse>> GetAllClinics();
        Task<List<ClinicResponse>?> GetFilterdClinics(string? searchBy, string? searchString);
    }
}
