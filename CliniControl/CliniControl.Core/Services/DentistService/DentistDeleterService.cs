using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.DentistContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.DentistService
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
