using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.DentistDTO;
using OdontoControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.DentistContracts
{
    public interface IDentistSorterService
    {
        List<DentistResponse>? GetSortedDentists(List<DentistResponse>? allDentist, string? sortBy, SortOrderOptions sortOrder);
    }
}
