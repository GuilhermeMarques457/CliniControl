using OdontoControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Domain.RepositoryContracts
{
    public interface IReminderRepository
    {
        Task<Reminder?> AddReminder(Reminder reminder);
        Task<Reminder?> UpdateReminder(Reminder reminder);
        Task<List<Reminder>?> GetReminderByDateTime(DateTime? date);
        Task<Reminder?> GetReminderByID(Guid? ID);
    }
}
