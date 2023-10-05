using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.PatientContracts
{
    public interface IPatientSorterService
    {
        List<PatientResponse>? GetSortedPatients(List<PatientResponse>? allPatient, string? sortBy, SortOrderOptions sortOrder);
    }
}
