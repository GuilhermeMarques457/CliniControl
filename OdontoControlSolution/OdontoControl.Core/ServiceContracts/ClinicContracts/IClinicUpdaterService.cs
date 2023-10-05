using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.ClinicContracts
{
    public interface IClinicUpdaterService
    {
        Task<ClinicResponse> UpdateClinic(ClinicUpdateRequest clinic);
    }
}
