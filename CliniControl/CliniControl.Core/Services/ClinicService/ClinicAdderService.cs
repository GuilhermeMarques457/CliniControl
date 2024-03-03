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
    public class ClinicAdderService : IClinicAdderService
    {
        private readonly IClinicRepository _repository;

        public ClinicAdderService(IClinicRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClinicResponse> AddClinic(ClinicAddRequest clinicRequest)
        {
            if(clinicRequest == null) 
                throw new ArgumentNullException(nameof(Clinic));

            Clinic clinic = clinicRequest.ToClinic();
            clinic.ID = Guid.NewGuid();

            await _repository.AddClinic(clinic);

            return clinic.ToClinicResponse();

        }
 
    }
}
