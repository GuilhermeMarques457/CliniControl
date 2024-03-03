using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.AppointmentService
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
