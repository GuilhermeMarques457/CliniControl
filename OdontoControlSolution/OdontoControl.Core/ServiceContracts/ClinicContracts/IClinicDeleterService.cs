using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.ClinicContracts
{
    public interface IClinicDeleterService
    {
        Task<bool> DeleteClinic(Guid? clincID);
    }
}
