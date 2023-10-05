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
