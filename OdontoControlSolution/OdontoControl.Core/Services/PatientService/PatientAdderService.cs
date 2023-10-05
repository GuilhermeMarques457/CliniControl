using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.Core.ServiceContracts.PatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.PatientService
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
