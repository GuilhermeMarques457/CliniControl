using CliniControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Domain.RepositoryContracts
{
    public interface IClinicRepository
    {
        Task<Clinic?> AddClinic(Clinic clinic);
        Task<Clinic?> UpdateClinic(Clinic clinic);
        Task<bool> DeleteClinic(Guid? clinicID);
        Task<Clinic?> GetClinicById(Guid? clinicID);
        Task<Clinic?> GetClinicByName(string clinicName);
        Task<List<Clinic>> GetAllClinics();
        Task<List<Clinic>> GetFilteredClinics(Expression<Func<Clinic, bool>> predicate);
    }
}
