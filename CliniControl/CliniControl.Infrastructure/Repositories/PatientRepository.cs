﻿using Microsoft.EntityFrameworkCore;
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
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> AddPatient(Patient Patient)
        {
            await _context.Patients.AddAsync(Patient);

            await _context.SaveChangesAsync();

            return Patient;
        }

        public async Task<bool> DeletePatient(Guid? PatientID)
        {
            Patient? Patient = await GetPatientById(PatientID);

            if (Patient == null) return false;

            _context.Patients.Remove(Patient);

            int rowsAfected = await _context.SaveChangesAsync();

            return rowsAfected > 0;
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            return await _context.Patients.Include("Manager").ToListAsync();
        }

        public async Task<Patient?> GetPatientById(Guid? PatientID)
        {
            return await _context.Patients.Include("Manager").AsNoTracking().FirstOrDefaultAsync(temp => temp.ID == PatientID);
        }

        public async Task<Patient?> UpdatePatient(Patient Patient)
        {
            _context.ChangeTracker.Clear();

            Patient? matchingPatient = await GetPatientById(Patient.ID);

            if (matchingPatient == null) return null;

            if (Patient.PhotoPath != null) matchingPatient.PhotoPath = Patient.PhotoPath;

            matchingPatient.Gender = Patient.Gender;
            matchingPatient.PatientName = Patient.PatientName;
            matchingPatient.PhoneNumber = Patient.PhoneNumber;
            matchingPatient.CPF = Patient.CPF;

            _context.Patients.Update(matchingPatient);

            await _context.SaveChangesAsync();

            return matchingPatient;
   
        }

        public async Task<List<Patient>> GetFilteredPatients(Expression<Func<Patient, bool>> predicate)
        {
            return await _context.Patients.Include("Manager").Where(predicate).ToListAsync();
        }

        public async Task<List<Patient>> GetPatientsByManagerId(Guid? managerID)
        {
            return await _context.Patients.Include("Manager").Where(temp => temp.ManagerID == managerID).ToListAsync();
        }

        public async Task<bool> DeletePatientAppointments(Guid? PatientID)
        {
            List<Appointment> dentistAppointments = _context.Appointments.Where(temp => temp.PatientID == PatientID).ToList();
            _context.Appointments.RemoveRange(dentistAppointments);

            int rowsAfected = await _context.SaveChangesAsync();

            return true;
        }
    }
}
