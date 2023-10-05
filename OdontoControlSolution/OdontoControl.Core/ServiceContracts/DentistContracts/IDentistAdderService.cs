using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.DentistDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.DentistContracts
{
    public interface IDentistAdderService
    {
        Task<DentistResponse> AddDentist(DentistAddRequest dentist);
    }
}
