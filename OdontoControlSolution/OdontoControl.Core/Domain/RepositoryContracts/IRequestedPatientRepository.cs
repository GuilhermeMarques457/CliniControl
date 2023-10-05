using OdontoControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Domain.RepositoryContracts
{
    public interface IRequestedPatientRepository
    {
        Task<RequestedPatient?> AddPatient(RequestedPatient Patient);
        Task<RequestedPatient?> UpdateContactedStatusPatient(RequestedPatient Patient);
        Task<List<RequestedPatient>?> GetAllNotContactedPatients(Guid? ClinicID);
        Task<RequestedPatient?> GetRequestedPatientByID(Guid? ID);
    }
}
