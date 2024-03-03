using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniControl.Core.ServiceContracts.PatientContracts
{
    public interface IPatientDeleterService
    {
        Task<bool> DeletePatient(Guid? PatientID);
        Task<bool> DeletePatientAppointments(Guid? PatientID);
    }
}
