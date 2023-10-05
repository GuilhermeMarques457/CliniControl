using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.ServiceContracts.TwilioContracts
{
    public interface ITwilioService
    {
        public bool SendWhatssapMessage(string? clientNumber, string? message);

    }
}
