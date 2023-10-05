using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.DTO.RequestedPatientDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.PatientContracts;
using OdontoControl.Core.ServiceContracts.RequestedPatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.RequestedPatientService
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
