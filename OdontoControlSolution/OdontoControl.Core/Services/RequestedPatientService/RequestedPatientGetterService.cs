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
    public class RequestedPatientGetterService : IRequestedPatientGetterService
    {
        private readonly IRequestedPatientRepository _repository;

        public RequestedPatientGetterService(IRequestedPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RequestedPatientResponse>?> GetAllNotContactedPatients(Guid? clinicID)
        {
            if (clinicID == null)
                return null;

            List<RequestedPatient>? patientsNotContacted = await _repository.GetAllNotContactedPatients(clinicID);

            if (patientsNotContacted == null) 
                return null;

            return patientsNotContacted.Select(temp => temp.ToRequestedPatientResponse()).ToList();
        }

        public async Task<RequestedPatientResponse?> GetRequestedPatientByID(Guid? patientID)
        {
            if (patientID == null)
                return null;

            RequestedPatient? patientsNotContacted = await _repository.GetRequestedPatientByID(patientID);

            if (patientsNotContacted == null)
                return null;

            return patientsNotContacted.ToRequestedPatientResponse();
        }
    }
}
