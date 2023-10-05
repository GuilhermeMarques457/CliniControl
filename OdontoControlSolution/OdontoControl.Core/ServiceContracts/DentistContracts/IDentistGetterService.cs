using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.DentistDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.DentistContracts
{
    public interface IDentistGetterService
    {
        Task<DentistResponse?> GetDentistById(Guid? dentistID);
        Task<List<DentistResponse>> GetAllDentists();
        Task<List<DentistResponse>?> GetFilterdDentists(string? searchBy, string? searchString);
        Task<List<DentistResponse>?> GetDentistsByClinicID(Guid? ClinicID);
    }
}
