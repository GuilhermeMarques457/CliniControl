using Microsoft.EntityFrameworkCore;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Infrastructure.Repositories
{
    public class DentistRepository : IDentistRepository
    {
        private readonly ApplicationDbContext _context;

        public DentistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dentist?> AddDentist(Dentist dentist)
        {
            await _context.Dentists.AddAsync(dentist);

            await _context.SaveChangesAsync();

            return dentist;
        }

        public async Task<bool> DeleteDentist(Guid? dentistID)
        {
            Dentist dentist = await _context.Dentists.FirstAsync(temp => temp.ID == dentistID);

            _context.Dentists.Remove(dentist);

            int rowsAfected = await _context.SaveChangesAsync();

            return rowsAfected > 0;
        }

        public async Task<List<Dentist>> GetAllDentists()
        {
            return await _context.Dentists.Include("Manager").ToListAsync();
        }

        public async Task<Dentist?> GetDentistById(Guid? dentistID)
        {
            return await _context.Dentists.Include(dent => dent.Manager)
                .FirstOrDefaultAsync(temp => temp.ID == dentistID);
        }

        public async Task<Dentist?> UpdateDentist(Dentist dentist)
        {
            _context.ChangeTracker.Clear();

            Dentist? matchingDentist = await GetDentistById(dentist.ID);

            if (matchingDentist == null) return null;

            if (dentist.PhotoPath != null) matchingDentist.PhotoPath = dentist.PhotoPath;

            matchingDentist.DentistName = dentist.DentistName;
            matchingDentist.PhoneNumber = dentist.PhoneNumber;
            matchingDentist.StartTime = dentist.StartTime;
            matchingDentist.EndTime = dentist.EndTime;

            _context.Dentists.Update(matchingDentist);

            await _context.SaveChangesAsync();

            return matchingDentist;

        }

        public async Task<List<Dentist>> GetFilteredDentists(Expression<Func<Dentist, bool>> predicate)
        {
            return await _context.Dentists.Where(predicate).ToListAsync();
        }

        public async Task<List<Dentist>> GetDentistsByClinicId(Guid? clinicID)
        {
            return await _context.Dentists.Include(d => d.Manager).Where(temp => temp.Manager!.ClinicID == clinicID).ToListAsync();
        }

        public async Task<bool> DeleteDentistAppointment(Guid? dentistID)
        {
            List<Appointment> dentistAppointments = _context.Appointments.Where(temp => temp.DentistID == dentistID).ToList();
            _context.Appointments.RemoveRange(dentistAppointments);

            int rowsAfected = await _context.SaveChangesAsync();

            return true;
        }
    }
}
