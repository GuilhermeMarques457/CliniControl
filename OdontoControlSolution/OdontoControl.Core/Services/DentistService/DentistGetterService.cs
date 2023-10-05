using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.DentistDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.DentistContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.DentistService
{
    public class DentistGetterService : IDentistGetterService
    {
        private readonly IDentistRepository _repository;

        public DentistGetterService(IDentistRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DentistResponse>> GetAllDentists()
        {
            List<Dentist> Dentist = await _repository.GetAllDentists();

            return Dentist.Select(temp => temp.ToDentistResponse()).ToList();
        }

        public async Task<DentistResponse?> GetDentistById(Guid? DentistID)
        {
            if (DentistID == null)
                return null;

            Dentist? Dentist = await _repository.GetDentistById(DentistID);

            if (Dentist == null)
                return null;

            return Dentist.ToDentistResponse();
        }

        public async Task<List<DentistResponse>?> GetDentistsByClinicID(Guid? ClinicID)
        {
            if (ClinicID == null)
                return null;

            List<Dentist>? dentistList = await _repository.GetDentistsByClinicId(ClinicID);

            if(dentistList == null)
                return null;

            return dentistList.Select(temp => temp.ToDentistResponse()).ToList();

        }

        public async Task<List<DentistResponse>?> GetFilterdDentists(string? searchBy, string? searchString)
        {

            List<Dentist>? Dentists = new List<Dentist>();

            if (searchString == null)
            {
                Dentists = await _repository.GetAllDentists();
            }
            else
            {

                Dentists = searchBy switch
                {
                    nameof(DentistResponse.DentistName) =>
                        await _repository.GetFilteredDentists(temp =>
                            temp.DentistName!.Contains(searchString)),
                    nameof(DentistResponse.PhoneNumber) =>
                        await _repository.GetFilteredDentists(temp =>
                            temp.PhoneNumber!.Contains(searchString)),
                    nameof(DentistResponse.StartTime) =>
                        await _repository.GetFilteredDentists(temp =>
                            temp.StartTime.ToString()!.Contains(searchString)),
                    nameof(DentistResponse.EndTime) =>
                        await _repository.GetFilteredDentists(temp =>
                            temp.EndTime.ToString()!.Contains(searchString)),
                    nameof(DentistResponse.Manager) =>
                        await _repository.GetFilteredDentists(temp =>
                            temp.Manager!.PersonName!.Contains(searchString)),
                    _ => await _repository.GetAllDentists()

                };
            }

            return Dentists.Select(temp => temp.ToDentistResponse()).ToList();
        }

       
    }
}
