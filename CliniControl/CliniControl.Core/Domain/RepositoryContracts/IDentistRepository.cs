using CliniControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.Domain.RepositoryContracts
{
    public interface IDentistRepository
    {
        Task<Dentist?> AddDentist(Dentist dentist);
        Task<Dentist?> UpdateDentist(Dentist dentist);
        Task<bool> DeleteDentist(Guid? dentistID);
        Task<Dentist?> GetDentistById(Guid? dentistID);
        Task<List<Dentist>> GetAllDentists();
        Task<List<Dentist>> GetFilteredDentists(Expression<Func<Dentist, bool>> predicate);
        Task<List<Dentist>> GetDentistsByClinicId(Guid? clinicID);
        Task<bool> DeleteDentistAppointment(Guid? dentistID);
    }
}
