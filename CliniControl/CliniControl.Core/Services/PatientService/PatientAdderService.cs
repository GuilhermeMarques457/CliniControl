using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.PatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.PatientService
{
    public class PatientAdderService : IPatientAdderService
    {
        private readonly IPatientRepository _repository;

        public PatientAdderService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatientResponse> AddPatient(PatientAddRequest PatientRequest)
        {
            if(PatientRequest == null) 
                throw new ArgumentNullException(nameof(PatientRequest));

            Patient Patient = PatientRequest.ToPatient();
            Patient.ID = Guid.NewGuid();

            await _repository.AddPatient(Patient);

            return Patient.ToPatientResponse();

        }
 
    }
}
