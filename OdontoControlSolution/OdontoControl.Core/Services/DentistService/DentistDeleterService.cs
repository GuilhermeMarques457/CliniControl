using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.DentistContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.DentistService
{
    public class DentistDeleterService : IDentistDeleterService
    {
        private readonly IDentistRepository _repository;


        public DentistDeleterService(IDentistRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> DeleteDentist(Guid? DentistID)
        {
            if(DentistID == null)
                throw new ArgumentNullException(nameof(DentistID));

            Dentist? existingDentist = await _repository.GetDentistById(DentistID);

            if (existingDentist == null)
                return false;

            return await _repository.DeleteDentist(DentistID);
        }

        public async Task<bool> DeleteDentistAppointments(Guid? dentistID)
        {
            if (dentistID == null)
                throw new ArgumentNullException(nameof(dentistID));

            Dentist? existingDentist = await _repository.GetDentistById(dentistID);

            if (existingDentist == null)
                return false;

            return await _repository.DeleteDentistAppointment(dentistID);
        }
    }
}
