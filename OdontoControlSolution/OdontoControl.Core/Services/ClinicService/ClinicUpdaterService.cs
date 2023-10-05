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
    public class ClinicUpdaterService : IClinicUpdaterService
    {
        private readonly IClinicRepository _repository;

        public ClinicUpdaterService(IClinicRepository repository)
        {
            _repository = repository;
        }

        

        public async Task<ClinicResponse> UpdateClinic(ClinicUpdateRequest clinic)
        {
            if (clinic == null)
                throw new ArgumentNullException(nameof(Clinic));

            Clinic? existingClinic = await _repository.GetClinicById(clinic.ID);

            if(existingClinic == null)
                throw new ArgumentException(nameof(existingClinic));

            Clinic? updatedClinic = await _repository.UpdateClinic(clinic.ToClinic()); 

            if (updatedClinic == null)
                throw new ArgumentException(nameof(updatedClinic));

            return updatedClinic.ToClinicResponse();
        }
    }
}
