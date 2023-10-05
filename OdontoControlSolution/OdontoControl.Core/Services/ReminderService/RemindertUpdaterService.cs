using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ReminderDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ReminderContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Services.ReminderService
{
    public class ReminderUpdaterService : IReminderUpdaterService
    {
        private readonly IReminderRepository _repository;

        public ReminderUpdaterService(IReminderRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReminderResponse?> UpdateReminder(ReminderUpdateRequest? Reminder)
        {
            if (Reminder == null)
                return null;

            Reminder? existingReminder = await _repository.GetReminderByID(Reminder.ID);

            if (existingReminder == null)
                return null;

            Reminder? updatedReminder = await _repository.UpdateReminder(Reminder.ToReminder());

            if (updatedReminder == null)
                return null;

            return updatedReminder.ToReminderResponse();
        }
    }
}
