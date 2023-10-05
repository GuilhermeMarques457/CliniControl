using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentSorterService
    {
        List<AppointmentResponse>? GetSortedAppointment(List<AppointmentResponse>? allAppointment, string? sortBy, SortOrderOptions sortOrder);
    }
}
