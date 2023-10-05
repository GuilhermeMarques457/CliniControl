using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.ReminderDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.Core.ServiceContracts.ReminderContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.ReminderService
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
