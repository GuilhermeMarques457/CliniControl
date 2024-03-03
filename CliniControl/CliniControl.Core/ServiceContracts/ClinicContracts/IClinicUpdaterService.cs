using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.ClinicContracts
{
    public interface IClinicUpdaterService
    {
        Task<ClinicResponse> UpdateClinic(ClinicUpdateRequest clinic);
    }
}
