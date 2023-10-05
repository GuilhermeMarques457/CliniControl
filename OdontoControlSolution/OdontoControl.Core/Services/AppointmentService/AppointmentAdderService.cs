using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.AppointmentService
{
    public class AppointmentAdderService : IAppointmentAdderService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentAdderService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<AppointmentResponse> AddAppointment(AppointmentAddRequest appointmentRequest)
        {
            if(appointmentRequest == null) 
                throw new ArgumentNullException(nameof(appointmentRequest));

            Appointment appointment = appointmentRequest.ToAppointment();
            appointment.ID = Guid.NewGuid();

            await _repository.AddAppointment(appointment);

            return appointment.ToAppointmentResponse();

        }
 
    }
}
