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
    public class ClinicRepository : IClinicRepository
    {
        private readonly ApplicationDbContext _context;

        public ClinicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Clinic?> AddClinic(Clinic clinic)
        {
            await _context.Clinics.AddAsync(clinic);

            await _context.SaveChangesAsync();

            return clinic;
        }

        public async Task<bool> DeleteClinic(Guid? clinicID)
        {
            Clinic clinic = await _context.Clinics.FirstAsync(temp => temp.ID == clinicID);

            _context.Clinics.Remove(clinic);

            int rowsAfected = await _context.SaveChangesAsync();

            return rowsAfected > 0;
        }

        public async Task<List<Clinic>> GetAllClinics()
        {
            return await _context.Clinics.ToListAsync();
        }

        public async Task<Clinic?> GetClinicById(Guid? clinicID)
        {
            return await _context.Clinics.FirstOrDefaultAsync(temp => temp.ID == clinicID);
        }

        public async Task<Clinic?> GetClinicByName(string clinicName)
        {
            return await _context.Clinics.FirstOrDefaultAsync(temp => temp.ClinicName == clinicName);
        }

        public async Task<Clinic?> UpdateClinic(Clinic clinic)
        {
            Clinic? matchingClinic = await _context.Clinics.FirstOrDefaultAsync(temp => temp.ID == clinic.ID);

            if (matchingClinic != null)
            {
                matchingClinic.StreetName = clinic.StreetName;
                matchingClinic.CNPJ = clinic.CNPJ;
                matchingClinic.City = clinic.City;
                matchingClinic.ClinicName = clinic.ClinicName;
                matchingClinic.Neighborhood = clinic.Neighborhood;
                matchingClinic.ID = clinic.ID;
                matchingClinic.Phone = clinic.Phone;
            }

            await _context.SaveChangesAsync();

            return matchingClinic;
   
        }

        public async Task<List<Clinic>> GetFilteredClinics(Expression<Func<Clinic, bool>> predicate)
        {
            return await _context.Clinics.Where(predicate).ToListAsync();
        }
    }
}
