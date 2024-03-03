using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.PatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.PatientService
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
