using OdontoControl.Core.ServiceContracts.TwilioContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Text.RegularExpressions;

namespace OdontoControl.Core.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly string TwilioAccountSid = "AC8754884aa48c6a182ba1e93ce0f69587";
        private readonly string TwilioAuthToken = "5064a015961eb6a304865efa52f9d0b1";

        public bool SendWhatssapMessage(string? clientNumber, string? message)
        {
            try
            {
                TwilioClient.Init(TwilioAccountSid, TwilioAuthToken);

                //string clientNumberFormatted = Regex.Replace(clientNumber, @"[^\d]", "");

                var messageSent = MessageResource.Create(
                    body: "Hello there!",
                    from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:+5518997354562")
                );

                return true;
            }catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            

        }

    }
}
