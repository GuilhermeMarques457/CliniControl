using Microsoft.EntityFrameworkCore;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Infrastructure.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ApplicationDbContext _context;

        public ReminderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reminder?> AddReminder(Reminder Reminder)
        {
            await _context.Reminders.AddAsync(Reminder);

            await _context.SaveChangesAsync();

            return Reminder;
        }

        public async Task<List<Reminder>?> GetReminderByDateTime(DateTime? today)
        {
            return await _context.Reminders.Where(temp => temp.ActityDate == today).ToListAsync();
        }

        public async Task<Reminder?> GetReminderByID(Guid? ID)
        {
            Reminder? Reminder = await _context.Reminders.FirstOrDefaultAsync(temp => temp.ID == ID);

            return Reminder;
        }

        public async Task<Reminder?> UpdateReminder(Reminder reminder)
        {
            _context.ChangeTracker.Clear();

            Reminder? matchingReminder = await GetReminderByID(reminder.ID);

            if (matchingReminder == null) return null;
            
            matchingReminder.Finished = reminder.Finished;
            matchingReminder.ActivityDescription = reminder.ActivityDescription;
            matchingReminder.ActityDate = reminder.ActityDate;

            _context.Reminders.Update(matchingReminder);

            await _context.SaveChangesAsync();

            return matchingReminder;
        }
    }
}
