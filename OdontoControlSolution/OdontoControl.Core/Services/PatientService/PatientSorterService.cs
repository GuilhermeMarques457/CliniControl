using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.Enums;
using OdontoControl.Core.ServiceContracts.PatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.PatientService
{
    public class PatientSorterService : IPatientSorterService
    {
        private readonly IPatientRepository _repository;

        public PatientSorterService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public List<PatientResponse>? GetSortedPatients(List<PatientResponse>? allPatients, string? sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPatients;

            List<PatientResponse>? sortedPatients = (sortBy, sortOrder)
                switch
                {
                    (nameof(PatientResponse.PatientName), SortOrderOptions.ASC)
                        => allPatients?.OrderBy(temp => temp.PatientName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.PatientName), SortOrderOptions.DESC)
                        => allPatients?.OrderByDescending(temp => temp.PatientName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.PhoneNumber), SortOrderOptions.ASC)
                        => allPatients?.OrderBy(temp => temp.PhoneNumber, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.PhoneNumber), SortOrderOptions.DESC)
                        => allPatients?.OrderByDescending(temp => temp.PhoneNumber, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.CPF), SortOrderOptions.ASC)
                        => allPatients?.OrderBy(temp => temp.CPF!.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.CPF), SortOrderOptions.DESC)
                        => allPatients?.OrderByDescending(temp => temp.CPF!.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.Gender), SortOrderOptions.ASC)
                        => allPatients?.OrderBy(temp => temp.Gender!.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.Gender), SortOrderOptions.DESC)
                        => allPatients?.OrderByDescending(temp => temp.Gender!.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.Manager.PersonName), SortOrderOptions.ASC)
                        => allPatients?.OrderBy(temp => temp!.Manager!.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PatientResponse.Manager.PersonName), SortOrderOptions.DESC)
                        => allPatients?.OrderByDescending(temp => temp.Manager!.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),                    
                    _ => allPatients,
                };

            return sortedPatients;
        }
    }
}
