using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.AppointmentContracts
{
    public interface IAppointmentSorterService
    {
        List<AppointmentResponse>? GetSortedAppointment(List<AppointmentResponse>? allAppointment, string? sortBy, SortOrderOptions sortOrder);
    }
}
