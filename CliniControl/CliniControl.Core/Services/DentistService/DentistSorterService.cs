using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.DentistDTO;
using CliniControl.Core.Enums;
using CliniControl.Core.ServiceContracts.DentistContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.DentistService
{
    public class DentistSorterService : IDentistSorterService
    {
        private readonly IDentistRepository _repository;

        public DentistSorterService(IDentistRepository repository)
        {
            _repository = repository;
        }

        public List<DentistResponse>? GetSortedDentists(List<DentistResponse>? allDentists, string? sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allDentists;

            List<DentistResponse>? sortedDentists = (sortBy, sortOrder)
                switch
                {
                    (nameof(DentistResponse.DentistName), SortOrderOptions.ASC)
                        => allDentists?.OrderBy(temp => temp.DentistName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.DentistName), SortOrderOptions.DESC)
                        => allDentists?.OrderByDescending(temp => temp.DentistName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.PhoneNumber), SortOrderOptions.ASC)
                        => allDentists?.OrderBy(temp => temp.PhoneNumber, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.PhoneNumber), SortOrderOptions.DESC)
                        => allDentists?.OrderByDescending(temp => temp.PhoneNumber, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.StartTime), SortOrderOptions.ASC)
                        => allDentists?.OrderBy(temp => temp.StartTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.StartTime), SortOrderOptions.DESC)
                        => allDentists?.OrderByDescending(temp => temp.StartTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.EndTime), SortOrderOptions.ASC)
                        => allDentists?.OrderBy(temp => temp.EndTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.EndTime), SortOrderOptions.DESC)
                        => allDentists?.OrderByDescending(temp => temp.EndTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.Manager.UserName), SortOrderOptions.ASC)
                        => allDentists?.OrderBy(temp => temp!.Manager!.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(DentistResponse.Manager.UserName), SortOrderOptions.DESC)
                        => allDentists?.OrderByDescending(temp => temp.Manager!.UserName, StringComparer.OrdinalIgnoreCase).ToList(),                    
                    _ => allDentists,
                };

            return sortedDentists;
        }
    }
}
