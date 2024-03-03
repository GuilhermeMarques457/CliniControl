using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.PatientContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.PatientService
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
