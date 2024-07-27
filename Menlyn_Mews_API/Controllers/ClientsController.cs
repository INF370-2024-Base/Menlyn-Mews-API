using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Client;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public ClientsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;
        }

        [HttpGet]

        public async Task<ActionResult> GetClients()
        {
            try
            {
                var clients = await _repository.GetClientsAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int clientId)
        {
            try
            {
                var clients = await _repository.GetClientByIdAsync(clientId);
                if (clients == null) return NotFound("Client Does Not Exist");
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
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
                clients.Client_Address = cvm.Client_Address;
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

        [HttpPost]
        public async Task<IActionResult> PostClient(ClientViewModel cvm)
        {
            var client = new Client
            {
                Client_Name = cvm.Client_Name,
                Client_Surname = cvm.Client_Surname,
                Client_ID_Number = cvm.Client_ID_Number,
                Client_Email_Address = cvm.Client_Email_Address,
                Client_Contact_Number = cvm.Client_Contact_Number,
                Client_Gender = cvm.Client_Gender,  
                Client_Address = cvm.Client_Address,
                Title = cvm.Title,
            };

            try
            {
                _repository.Add(client);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(client);
        }

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

    }
}
