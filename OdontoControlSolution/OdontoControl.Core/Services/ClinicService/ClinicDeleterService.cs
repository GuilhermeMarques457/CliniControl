using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.ClinicService
{
    public class ClinicDeleterService : IClinicDeleterService
    {
        private readonly IClinicRepository _repository;

        public ClinicDeleterService(IClinicRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> DeleteClinic(Guid? clinicID)
        {
            if(clinicID == null)
                throw new ArgumentNullException(nameof(clinicID));

            Clinic? existingClinic = await _repository.GetClinicById(clinicID);

            if (existingClinic == null)
                return false;

            return await _repository.DeleteClinic(clinicID);
        }
    }
}
