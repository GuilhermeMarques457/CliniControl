using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.Helpers;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.AppointmentService
{
    public class AppointmentDeleterService : IAppointmentDeleterService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentDeleterService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> DeleteAppointment(Guid? appointmentID)
        {
            if(appointmentID == null)
                throw new ArgumentNullException(nameof(appointmentID));

            Appointment? existingAppointment = await _repository.GetAppointmentById(appointmentID);

            if (existingAppointment == null)
                return false;

            return await _repository.DeleteAppointment(appointmentID);
        }

        public async Task<bool> DeleteAttachment(string? urlPathImg, Guid? appointmentID, string? wwwrootPath)
        {
         
            if (appointmentID == null)
                throw new ArgumentNullException(nameof(appointmentID));

            if (urlPathImg == null)
                throw new ArgumentNullException(nameof(urlPathImg));

            bool finded = await _repository.DeleteAttachment(urlPathImg, appointmentID.Value);

            if (finded)
                await ManageImageProject.DeleteImage(urlPathImg, wwwrootPath);

            return finded;
        }
    }
}
