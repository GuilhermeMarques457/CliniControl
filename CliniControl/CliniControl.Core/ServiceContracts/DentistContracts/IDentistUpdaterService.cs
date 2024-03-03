using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.DentistDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.DentistContracts
{
    public interface IDentistUpdaterService
    {
        Task<DentistResponse> UpdateDentist(DentistUpdateRequest dentist);
    }
}
