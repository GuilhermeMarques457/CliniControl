using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.DTO.RequestedPatientDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.Core.ServiceContracts.PatientContracts;
using OdontoControl.Core.ServiceContracts.RequestedPatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.RequestedPatientService
{
    public class RequestedPatientAdderService : IRequestedPatientAdderService
    {
        private readonly IRequestedPatientRepository _repository;

        public RequestedPatientAdderService(IRequestedPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestedPatientResponse> AddPatient(RequestedPatientAddRequest? Patient)
        {
            if (Patient == null)
                throw new ArgumentNullException(nameof(Patient));

            RequestedPatient requestedPatient = Patient.ToPatient();
            requestedPatient.ID = Guid.NewGuid();

            await _repository.AddPatient(requestedPatient);

            return requestedPatient.ToRequestedPatientResponse();

        }
    }
}
