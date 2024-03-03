using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.DTO.RequestedPatientDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.PatientContracts;
using CliniControl.Core.ServiceContracts.RequestedPatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.RequestedPatientService
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
