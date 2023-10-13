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
    public class PatientGetterService : IPatientGetterService
    {
        private readonly IPatientRepository _repository;

        public PatientGetterService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PatientResponse>> GetAllPatients()
        {
            List<Patient> Patient = await _repository.GetAllPatients();

            return Patient.Select(temp => temp.ToPatientResponse()).ToList();
        }

        public async Task<PatientResponse?> GetPatientById(Guid? PatientID)
        {
            if (PatientID == null)
                return null;

            Patient? Patient = await _repository.GetPatientById(PatientID);

            if (Patient == null)
                return null;

            return Patient.ToPatientResponse();
        }



        public async Task<List<PatientResponse>?> GetPatientsByManagerID(Guid? ManagerID)
        {
            if (ManagerID == null)
                return null;

            List<Patient>? PatientList = await _repository.GetPatientsByManagerId(ManagerID);

            if(PatientList == null)
                return null;

            return PatientList.Select(temp => temp.ToPatientResponse()).ToList();

        }

        public async Task<List<PatientResponse>?> GetFilterdPatients(string? searchBy, string? searchString)
        {

            List<Patient>? Patients = new List<Patient>();

            if (searchString == null)
            {
                Patients = await _repository.GetAllPatients();
            }
            else
            {

                Patients = searchBy switch
                {
                    nameof(PatientResponse.PatientName) =>
                        await _repository.GetFilteredPatients(temp =>
                            temp.PatientName!.Contains(searchString)),
                    nameof(PatientResponse.PhoneNumber) =>
                        await _repository.GetFilteredPatients(temp =>
                            temp.PhoneNumber!.Contains(searchString)),
                    nameof(PatientResponse.ManagerID) =>
                        await _repository.GetFilteredPatients(temp =>
                            temp.ManagerID.ToString()!.Contains(searchString)),
                    nameof(PatientResponse.CPF) =>
                        await _repository.GetFilteredPatients(temp =>
                            temp.CPF!.Contains(searchString)),
                    nameof(PatientResponse.Gender) =>
                        await _repository.GetFilteredPatients(temp =>
                            temp.Gender!.Contains(searchString)),
                    _ => await _repository.GetAllPatients()

                };
            }

            return Patients.Select(temp => temp.ToPatientResponse()).ToList();
        }
    }
}
