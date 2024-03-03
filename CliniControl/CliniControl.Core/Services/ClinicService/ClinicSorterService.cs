using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.Enums;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.ClinicService
{
    public class ClinicSorterService : IClinicSorterService
    {
        private readonly IClinicRepository _repository;

        public ClinicSorterService(IClinicRepository repository)
        {
            _repository = repository;
        }

        public List<ClinicResponse>? GetSortedClinics(List<ClinicResponse>? allClinics, string? sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allClinics;

            List<ClinicResponse>? sortedClinics = (sortBy, sortOrder)
                switch
                {
                    (nameof(ClinicResponse.CNPJ), SortOrderOptions.ASC)
                        => allClinics?.OrderBy(temp => temp.CNPJ, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.CNPJ), SortOrderOptions.DESC)
                        => allClinics?.OrderByDescending(temp => temp.CNPJ, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.Phone), SortOrderOptions.ASC)
                        => allClinics?.OrderBy(temp => temp.Phone, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.Phone), SortOrderOptions.DESC)
                        => allClinics?.OrderByDescending(temp => temp.Phone, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.StreetName), SortOrderOptions.ASC)
                        => allClinics?.OrderBy(temp => temp.StreetName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.StreetName), SortOrderOptions.DESC)
                        => allClinics?.OrderByDescending(temp => temp.StreetName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.City), SortOrderOptions.ASC)
                        => allClinics?.OrderBy(temp => temp.City, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.City), SortOrderOptions.DESC)
                        => allClinics?.OrderByDescending(temp => temp.City, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.ClinicName), SortOrderOptions.ASC)
                        => allClinics?.OrderBy(temp => temp.ClinicName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.ClinicName), SortOrderOptions.DESC)
                        => allClinics?.OrderByDescending(temp => temp.ClinicName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.Neighborhood), SortOrderOptions.ASC)
                        => allClinics?.OrderBy(temp => temp.Neighborhood, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(ClinicResponse.Neighborhood), SortOrderOptions.DESC)
                        => allClinics?.OrderByDescending(temp => temp.Neighborhood, StringComparer.OrdinalIgnoreCase).ToList(),
                    _ => allClinics,
                };

            return sortedClinics;
        }
    }
}
