using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.PatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.PatientService
{
    public class PatientUpdaterService : IPatientUpdaterService
    {
        private readonly IPatientRepository _repository;

        public PatientUpdaterService(IPatientRepository repository)
        {
            _repository = repository;
        }

        

        public async Task<PatientResponse> UpdatePatient(PatientUpdateRequest Patient)
        {
            if (Patient == null)
                throw new ArgumentNullException(nameof(Patient));

            Patient? existingPatient = await _repository.GetPatientById(Patient.ID);

            if(existingPatient == null)
                throw new ArgumentException(nameof(existingPatient));

            Patient? updatedPatient = await _repository.UpdatePatient(Patient.ToPatient()); 

            if (updatedPatient == null)
                throw new ArgumentException(nameof(updatedPatient));

            return updatedPatient.ToPatientResponse();
        }
    }
}
