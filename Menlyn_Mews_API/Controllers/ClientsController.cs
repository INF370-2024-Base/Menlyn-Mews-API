using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Client;
using Menlyn_Mews_API.Models.Domain.Emails;
using Menlyn_Mews.Service.Services;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        private readonly IGeneralEmailService _generateEmailService;

        public ClientsController(AppDbContext context, IRepositroy repositroy, IGeneralEmailService generalEmailService)
        {
            _context = context;
            _repository = repositroy;
            _generateEmailService = generalEmailService;
        }



        [HttpGet]
        public async Task<ActionResult> GetClients()
        {
            try
            {
                var results = await _repository.GetClientsAsync();

                dynamic clients = results.Select(c => new
                {
                    c.ClientId,
                    c.Client_Name,
                    c.Client_Surname,
                    c.Client_ID_Number,
                    c.Client_Email_Address,
                    c.Client_Contact_Number,
                    c.Client_Gender,
                    c.Title,
                    c.ApplicationUserId,
                    Username = c.ApplicationUser.UserName,
                });

                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ContactUs/{email}/{name}/{message}")]
        public async Task<IActionResult> ContactUsEmail(string email, string name, string message)
        {
            var mailRequest = new Mailrequest
            {
                ToEmail = "menlynmews370@gmail.com",
                Subject = "Contact Us Request",
                Body = GenerateContactUsEmailBody(email, name, message)
            };

            try
            {
                await _generateEmailService.SendEmailAsync(mailRequest);

                return Ok(new { Message = "We Have Received Your Email! We Will Contact You When We Can!" });
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status400BadRequest,
                               new Response { Message = "Could Not Send Email!" });
            }

        }

        [HttpGet("{clientId}")]
        public async Task<ActionResult<Client>> GetClient(int clientId)
        {
            try
            {
                var c = await _repository.GetClientByIdAsync(clientId);
                if (c == null) return NotFound("Client Does Not Exist");

                dynamic clients = new
                {   
                    c.ClientId,
                    c.Client_Name,
                    c.Client_Surname,
                    c.Client_ID_Number,
                    c.Client_Email_Address,
                    c.Client_Contact_Number,
                    c.Client_Gender,
                    c.Title,
                    c.ApplicationUserId,
                };

                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{clientId}")]
        public async Task<ActionResult<ClientViewModel>> PutClient(int clientId, ClientViewModel cvm)
        {
            try
            {
                var clients = await _repository.GetClientByIdAsync(clientId);
                if (clients == null) return NotFound("Client Does Not Exist");

                clients.Client_Name = cvm.Client_Name;
                clients.Client_Surname = cvm.Client_Surname;
                clients.Client_ID_Number = cvm.Client_ID_Number;
                clients.Client_Email_Address = cvm.Client_Email_Address;
                clients.Client_Contact_Number = cvm.Client_Contact_Number;
                clients.Client_Gender = cvm.Client_Gender;
                clients.Title = cvm.Title;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(clients);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        //[HttpPost]
        //public async Task<IActionResult> PostClient(ClientViewModel cvm)
        //{
        //    var client = new Client
        //    {
        //        Client_Name = cvm.Client_Name,
        //        Client_Surname = cvm.Client_Surname,
        //        Client_ID_Number = cvm.Client_ID_Number,
        //        Client_Email_Address = cvm.Client_Email_Address,
        //        Client_Contact_Number = cvm.Client_Contact_Number,
        //        Client_Gender = cvm.Client_Gender,  
        //        Title = cvm.Title,
        //    };

        //    try
        //    {
        //        _repository.Add(client);
        //        await _repository.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    return Ok(client);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string GenerateContactUsEmailBody(string name, string userEmail, string messageContent)
        {
            var htmlContent = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    width: 100%;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    max-width: 600px;
                    margin: 50px auto;
                }}
                .header {{
                    background-color: #6a329f;
                    color: #ffffff;
                    padding: 20px;
                    text-align: center;
                    border-top-left-radius: 10px;
                    border-top-right-radius: 10px;
                }}
                .content {{
                    padding: 20px;
                    font-size: 16px;
                    line-height: 1.6;
                    color: #333333;
                }}
                .content p {{
                    margin-bottom: 20px;
                }}
                .message {{
                    padding: 15px;
                    background-color: #f9f9f9;
                    border-left: 4px solid #6a329f;
                    font-style: italic;
                }}
                .footer {{
                    padding: 20px;
                    text-align: center;
                    font-size: 14px;
                    color: #888888;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>New Contact Us Request</h1>
                </div>
                <div class='content'>
                    <p><strong>Name:</strong> {name}</p>
                    <p><strong>Email:</strong> {userEmail}</p>
                    <p><strong>Message:</strong></p>
                    <div class='message'>
                        <p>{messageContent}</p>
                    </div>
                </div>
                <div class='footer'>
                    <p>This email was sent from the Menlyn Mews website.</p>
                </div>
            </div>
        </body>
        </html>";

            return htmlContent;
        }


    }
}
