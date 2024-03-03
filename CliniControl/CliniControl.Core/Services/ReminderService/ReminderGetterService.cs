using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ReminderDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ReminderContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Services.ReminderService
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
