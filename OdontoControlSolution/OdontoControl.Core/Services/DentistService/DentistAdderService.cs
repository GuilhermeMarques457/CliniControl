using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.DentistDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.Core.ServiceContracts.DentistContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.DentistService
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
