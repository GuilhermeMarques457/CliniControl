using Microsoft.EntityFrameworkCore;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Infrastructure.Repositories
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
            return await _context.Dentists.FirstOrDefaultAsync(temp => temp.ID == dentistID);
        }

        public async Task<Dentist?> UpdateDentist(Dentist dentist)
        {
            Dentist? matchingDentist = await _context.Dentists.FirstOrDefaultAsync(temp => temp.ID == dentist.ID);

            if (matchingDentist != null)
            {
                matchingDentist.ID = dentist.ID;
                matchingDentist.DentistName = dentist.DentistName;
                matchingDentist.PhoneNumber = dentist.PhoneNumber;
            }

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
