using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.ClinicService
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
