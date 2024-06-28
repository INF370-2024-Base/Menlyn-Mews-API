using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendSMSController : ControllerBase
    {

        string accountSid = "AC68ce8e5c11a913eb26d112a30b19aabb";
        string authToken = "a88822c4277482823eef8666a52998c3";
        
        [HttpPost("SendText")]
        public ActionResult SendText(string phoneNumber)
        {
            TwilioClient.Init(accountSid, authToken); 

            var message = MessageResource.Create(
                body: "Your Check In Date Is 12-08-2024",
                from: new Twilio.Types.PhoneNumber("+13187034034"),
                to: new Twilio.Types.PhoneNumber("+27" + phoneNumber)
            );

            return StatusCode(200, new { message = message.Sid });
        }
    }
}
