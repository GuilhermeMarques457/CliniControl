using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.PatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.PatientService
{
    public class PatientDeleterService : IPatientDeleterService
    {
        private readonly IPatientRepository _repository;

        public PatientDeleterService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> DeletePatient(Guid? PatientID)
        {
            if(PatientID == null)
                throw new ArgumentNullException(nameof(PatientID));

            Patient? existingPatient = await _repository.GetPatientById(PatientID);

            if (existingPatient == null)
                return false;

            return await _repository.DeletePatient(PatientID);
        }

        public async Task<bool> DeletePatientAppointments(Guid? PatientID)
        {
            if (PatientID == null)
                throw new ArgumentNullException(nameof(PatientID));

            Patient? existingPatient = await _repository.GetPatientById(PatientID);

            if (existingPatient == null)
                return false;

            return await _repository.DeletePatientAppointments(PatientID);
        }
    }
}
