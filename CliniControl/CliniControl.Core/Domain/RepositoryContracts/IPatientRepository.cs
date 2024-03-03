using CliniControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Domain.RepositoryContracts
{
    public interface IPatientRepository
    {
        Task<Patient?> AddPatient(Patient Patient);
        Task<Patient?> UpdatePatient(Patient Patient);
        Task<bool> DeletePatient(Guid? PatientID);
        Task<Patient?> GetPatientById(Guid? PatientID);
        Task<List<Patient>> GetAllPatients();
        Task<List<Patient>> GetFilteredPatients(Expression<Func<Patient, bool>> predicate);
        Task<List<Patient>> GetPatientsByManagerId(Guid? managerID);
        Task<bool> DeletePatientAppointments(Guid? PatientID);
    }
}
