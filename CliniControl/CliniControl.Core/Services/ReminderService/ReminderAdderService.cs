using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.ReminderDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.ReminderContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.ReminderService
{
    public class ReminderAdderService : IReminderAdderService
    {
        private readonly IReminderRepository _repository;

        public ReminderAdderService(IReminderRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReminderResponse> AddReminder(ReminderAddRequest? reminder)
        {
            if (reminder == null)
                throw new ArgumentNullException(nameof(reminder));

            Reminder Reminder = reminder.ToReminder();
            Reminder.ID = Guid.NewGuid();

            await _repository.AddReminder(Reminder);

            return Reminder.ToReminderResponse();

        }
    }
}
