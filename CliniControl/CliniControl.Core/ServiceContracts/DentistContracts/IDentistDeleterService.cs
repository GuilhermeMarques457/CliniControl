using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.DentistContracts
{
    public interface IDentistDeleterService
    {
        Task<bool> DeleteDentist(Guid? dentistID);
        Task<bool> DeleteDentistAppointments(Guid? dentistID);
    }
}
