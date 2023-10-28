using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.Enums;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.AppointmentService
{
    public class AppointmentSorterService : IAppointmentSorterService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentSorterService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public List<AppointmentResponse>? GetSortedAppointment(List<AppointmentResponse>? allAppointment, string? sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allAppointment;

            List<AppointmentResponse>? sortedAppointment = (sortBy, sortOrder)
                switch
            {
                (nameof(AppointmentResponse.Patient.PatientName), SortOrderOptions.ASC)
                    => allAppointment?.OrderBy(temp => temp.Patient?.PatientName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.Patient.PatientName), SortOrderOptions.DESC)
                    => allAppointment?.OrderByDescending(temp => temp.Patient?.PatientName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.Dentist.DentistName), SortOrderOptions.ASC)
                => allAppointment?.OrderBy(temp => temp.Dentist?.DentistName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.Dentist.DentistName), SortOrderOptions.DESC)
                    => allAppointment?.OrderByDescending(temp => temp.Dentist?.DentistName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.StartTime), SortOrderOptions.ASC)
                    => allAppointment?.OrderBy(temp => temp.StartTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.StartTime), SortOrderOptions.DESC)
                    => allAppointment?.OrderByDescending(temp => temp.StartTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.EndTime), SortOrderOptions.ASC)
                    => allAppointment?.OrderBy(temp => temp.EndTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.EndTime), SortOrderOptions.DESC)
                    => allAppointment?.OrderByDescending(temp => temp.EndTime.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.Status), SortOrderOptions.ASC)
                    => allAppointment?.OrderBy(temp => temp.Status, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.Status), SortOrderOptions.DESC)
                    => allAppointment?.OrderByDescending(temp => temp.Status, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.AppointmentTime), SortOrderOptions.ASC)
                    => allAppointment?.OrderBy(temp => temp.AppointmentTime!.Value).ToList(),
                (nameof(AppointmentResponse.AppointmentTime), SortOrderOptions.DESC)
                    => allAppointment?.OrderByDescending(temp => temp.AppointmentTime!.Value.ToString()).ToList(),
                (nameof(AppointmentResponse.ProcedureType), SortOrderOptions.ASC)
                    => allAppointment?.OrderBy(temp => temp.ProcedureType, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(AppointmentResponse.ProcedureType), SortOrderOptions.DESC)
                    => allAppointment?.OrderByDescending(temp => temp.ProcedureType, StringComparer.OrdinalIgnoreCase).ToList(),
                _ => allAppointment,
            };

            return sortedAppointment;
        }

    }
}
