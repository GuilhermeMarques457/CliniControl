using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.DentistDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.DentistContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.DentistService
{
    public class DentistAdderService : IDentistAdderService
    {
        private readonly IDentistRepository _repository;

        public DentistAdderService(IDentistRepository repository)
        {
            _repository = repository;
        }

        public async Task<DentistResponse> AddDentist(DentistAddRequest dentistRequest)
        {
            if(dentistRequest == null) 
                throw new ArgumentNullException(nameof(Dentist));

            Dentist dentist = dentistRequest.ToDentist();
            dentist.ID = Guid.NewGuid();

            await _repository.AddDentist(dentist);

            return dentist.ToDentistResponse();

        }
 
    }
}
