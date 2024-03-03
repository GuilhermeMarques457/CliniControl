using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.DTO.RequestedPatientDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.PatientContracts;
using CliniControl.Core.ServiceContracts.RequestedPatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.RequestedPatientService
{
    public class RequestedPatientUpdaterService : IRequestedPatientUpdaterService
    {
        private readonly IRequestedPatientRepository _repository;

        public RequestedPatientUpdaterService(IRequestedPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestedPatientResponse> UpdateContactedStatusPatient(RequestedPatientUpdateRequest? patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(Patient));

            RequestedPatient? existingPatient = await _repository.GetRequestedPatientByID(patient.ID);

            if (existingPatient == null)
                throw new ArgumentException(nameof(existingPatient));

            existingPatient.Contacted = true;

            RequestedPatient? updatedPatient = await _repository.UpdateContactedStatusPatient(existingPatient);

            if (updatedPatient == null)
                throw new ArgumentException(nameof(updatedPatient));

            return updatedPatient.ToRequestedPatientResponse();
        }

    }
}
