using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.DentistDTO;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.DentistContracts
{
    public interface IDentistSorterService
    {
        List<DentistResponse>? GetSortedDentists(List<DentistResponse>? allDentist, string? sortBy, SortOrderOptions sortOrder);
    }
}
