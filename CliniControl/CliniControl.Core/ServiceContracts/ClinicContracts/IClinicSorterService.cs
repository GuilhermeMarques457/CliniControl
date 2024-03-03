using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.ClinicContracts
{
    public interface IClinicSorterService
    {
        List<ClinicResponse>? GetSortedClinics(List<ClinicResponse>? allClinics, string? sortBy, SortOrderOptions sortOrder);
    }
}
