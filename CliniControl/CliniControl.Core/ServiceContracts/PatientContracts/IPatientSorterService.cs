using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.PatientContracts
{
    public interface IPatientSorterService
    {
        List<PatientResponse>? GetSortedPatients(List<PatientResponse>? allPatient, string? sortBy, SortOrderOptions sortOrder);
    }
}
