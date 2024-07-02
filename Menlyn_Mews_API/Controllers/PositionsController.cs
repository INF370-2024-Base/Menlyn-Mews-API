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
using Menlyn_Mews_API.ViewModels.Employee;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;

        public PositionsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetPositions")]
        public async Task<ActionResult> GetPositions()
        {
            try
            {
                var positions = await _repository.GetPositionsAsync();
                return Ok(positions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetPositionById/{positionId}")]
        public async Task<ActionResult> GetPosition(int positionId)
        {
            try
            {
                var positions = await _repository.GetPositionByIdAsync(positionId);
                if (positions == null) return NotFound("Position Not Found");
                return Ok(positions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdatePosition/{positionId}")]
        public async Task<ActionResult<PositionViewModel>> PutPosition(int positionId, PositionViewModel pvm)
        {
            try
            {
                var positions = await _repository.GetPositionByIdAsync(positionId);
                if (positions == null) return NotFound("Position Not Found");

                positions.Position_Description = pvm.Position_Description;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(positions);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostPosition(PositionViewModel pvm)
        {
            var positions = new Position
            {
                Position_Description = pvm.Position_Description,  
            };

            try
            {
                _repository.Add(positions);
                await _repository.SaveChangesAsync();   
            }
            catch (Exception)
            {
                return BadRequest("Invalid Transaction");
            }

            return Ok(positions);   
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            if (_context.Positions == null)
            {
                return NotFound();
            }
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PositionExists(int id)
        {
            return (_context.Positions?.Any(e => e.PositionId == id)).GetValueOrDefault();
        }
    }
}
