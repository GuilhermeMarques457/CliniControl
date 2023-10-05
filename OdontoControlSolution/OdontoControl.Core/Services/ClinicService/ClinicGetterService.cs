using Microsoft.AspNetCore.JsonPatch.Operations;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.ClinicService
{
    public class ClinicGetterService : IClinicGetterService
    {
        private readonly IClinicRepository _repository;

        public ClinicGetterService(IClinicRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClinicResponse>> GetAllClinics()
        {
            List<Clinic> clinic = await _repository.GetAllClinics();

            return clinic.Select(temp => temp.ToClinicResponse()).ToList();
        }

        public async Task<ClinicResponse?> GetClinicById(Guid? clinicID)
        {
            if (clinicID == null)
                return null;

            Clinic? clinic = await _repository.GetClinicById(clinicID);

            if (clinic == null)
                return null;

            return clinic.ToClinicResponse();
        }

        public async Task<ClinicResponse?> GetClinicByName(string? clinicName)
        {
            if (clinicName == null)
                return null;

            Clinic? clinic = await _repository.GetClinicByName(clinicName);

            if (clinic == null)
                return null;

            return clinic.ToClinicResponse();
        }

        public async Task<List<ClinicResponse>?> GetFilterdClinics(string? searchBy, string? searchString)
        {

            List<Clinic>? clinics = new List<Clinic>();

            if (searchString == null)
            {
                clinics = await _repository.GetAllClinics();
            }
            else
            {

                clinics = searchBy switch
                {
                    nameof(ClinicResponse.CNPJ) =>
                        await _repository.GetFilteredClinics(temp =>
                            temp.CNPJ!.Contains(searchString)),
                    nameof(ClinicResponse.City) =>
                        await _repository.GetFilteredClinics(temp =>
                            temp.City!.Contains(searchString)),
                    nameof(ClinicResponse.ClinicName) =>
                        await _repository.GetFilteredClinics(temp =>
                            temp.ClinicName!.Contains(searchString)),
                    nameof(ClinicResponse.Neighborhood) =>
                        await _repository.GetFilteredClinics(temp =>
                            temp.Neighborhood!.Contains(searchString)),
                    nameof(ClinicResponse.Phone) =>
                        await _repository.GetFilteredClinics(temp =>
                            temp.Phone!.Contains(searchString)),
                    nameof(ClinicResponse.StreetName) =>
                        await _repository.GetFilteredClinics(temp =>
                            temp.StreetName!.Contains(searchString)),
                    _ => await _repository.GetAllClinics()

                };
            }

            return clinics.Select(temp => temp.ToClinicResponse()).ToList();
        }
    }
}
