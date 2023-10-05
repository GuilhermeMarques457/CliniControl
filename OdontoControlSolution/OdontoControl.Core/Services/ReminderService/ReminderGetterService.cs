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
    public class ReminderGetterService : IReminderGetterService
    {
        private readonly IReminderRepository _repository;

        public ReminderGetterService(IReminderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReminderResponse>?> GetReminderByDateTime(DateTime? date)
        {
            if (date == null)
                return null;

            List<Reminder>? reminderListOfThisDate = await _repository.GetReminderByDateTime(date);

            if(reminderListOfThisDate == null)
                return null;

            return reminderListOfThisDate.Select(temp => temp.ToReminderResponse()).ToList();
        }

        public async Task<ReminderResponse?> GetReminderByID(Guid? ReminderID)
        {
            if (ReminderID == null)
                return null;

            Reminder? RemindersNotContacted = await _repository.GetReminderByID(ReminderID);

            if (RemindersNotContacted == null)
                return null;

            return RemindersNotContacted.ToReminderResponse();
        }
    }
}
