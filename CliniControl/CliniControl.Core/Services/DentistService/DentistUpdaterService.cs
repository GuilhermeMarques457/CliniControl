using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.DentistDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.DentistContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.DentistService
{
    public class DentistUpdaterService : IDentistUpdaterService
    {
        private readonly IDentistRepository _repository;

        public DentistUpdaterService(IDentistRepository repository)
        {
            _repository = repository;
        }

        

        public async Task<DentistResponse> UpdateDentist(DentistUpdateRequest Dentist)
        {
            if (Dentist == null)
                throw new ArgumentNullException(nameof(Dentist));

            Dentist? existingDentist = await _repository.GetDentistById(Dentist.ID);

            if(existingDentist == null)
                throw new ArgumentException(nameof(existingDentist));

            Dentist? updatedDentist = await _repository.UpdateDentist(Dentist.ToDentist()); 

            if (updatedDentist == null)
                throw new ArgumentException(nameof(updatedDentist));

            return updatedDentist.ToDentistResponse();
        }
    }
}
