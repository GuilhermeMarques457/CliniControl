using Microsoft.EntityFrameworkCore;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Infrastructure.Repositories
{
    public class RequestedPatientRepository : IRequestedPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestedPatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RequestedPatient?> AddPatient(RequestedPatient Patient)
        {
            await _context.RequestedPatients.AddAsync(Patient);

            await _context.SaveChangesAsync();

            return Patient;
        }

        public async Task<List<RequestedPatient>?> GetAllNotContactedPatients(Guid? ClinicID)
        {
            List<RequestedPatient>? notContactedPatients = await _context.RequestedPatients.Where(temp => temp.Contacted == false && temp.ClinicID == ClinicID).ToListAsync();

            return notContactedPatients;
        }

        public async Task<RequestedPatient?> GetRequestedPatientByID(Guid? ID)
        {
            RequestedPatient? requestedPatient = await _context.RequestedPatients.FirstOrDefaultAsync(temp => temp.ID == ID);

            return requestedPatient;
        }

        public async Task<RequestedPatient?> UpdateContactedStatusPatient(RequestedPatient Patient)
        {
            RequestedPatient? matchingPatient = await _context.RequestedPatients.FirstOrDefaultAsync(temp => temp.ID == Patient.ID);

            if (matchingPatient != null)
            {
                matchingPatient.PatientName = Patient.PatientName;
                matchingPatient.PhoneNumber = Patient.PhoneNumber;
                matchingPatient.Contacted = Patient.Contacted;
            }

            await _context.SaveChangesAsync();

            return matchingPatient;
        }

    }
}
