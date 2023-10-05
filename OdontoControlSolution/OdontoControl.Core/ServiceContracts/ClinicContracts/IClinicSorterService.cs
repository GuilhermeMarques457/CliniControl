using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.ClinicContracts
{
    public interface IClinicSorterService
    {
        List<ClinicResponse>? GetSortedClinics(List<ClinicResponse>? allClinics, string? sortBy, SortOrderOptions sortOrder);
    }
}
