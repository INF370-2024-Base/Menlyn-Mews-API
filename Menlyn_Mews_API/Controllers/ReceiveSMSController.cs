using Menlyn_Mews_API.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiveSMSController : TwilioController
    {
        [HttpPost("SendReply")]
        public TwiMLResult SendReply([FromBody] TwilioSMS request)
        {
            var response = new MessagingResponse();
            response.Message("Test 123");

            return TwiML(response);
        }

    }
}
