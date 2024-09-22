using Microsoft.AspNetCore.Mvc;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.ViewModels.Booking;
using System.Net.Http.Headers;
using Menlyn_Mews_API.Models.Domain;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepositroy _repository;
        
        public RoomsController(AppDbContext context, IRepositroy repositroy)
        {
            _context = context;
            _repository = repositroy;   
        }

        [HttpGet]
        [Route("GetRooms")]
        public async Task<ActionResult> GetRooms()
        {
            try
            {
                var results = await _repository.GetRoomsAsync();

                dynamic rooms = results.Select(r => new
                {
                    r.RoomId,
                    r.Room_Number,
                    r.Room_Floor,
                    r.Room_Status,
                    r.Room_Rate,
                    r.Room_Description,
                    Room_Photo_1 = $"{Request.Scheme}://{Request.Host.Value}/{r.Room_Photo_1}",
                    r.Room_Photo_2,
                    r.Room_Photo_3,
                    Room_Type = r.Room_Type.Room_Type_Description,
                });

                return Ok(rooms);   

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("GetRoomDataById/{roomId}")]
        public async Task<ActionResult> GetRoomData(int roomId)
        {
            try
            {
                var r = await _repository.GetRoomByIdAsync(roomId);
                if (r == null) return NotFound("Room Does Not Exist");

                dynamic rooms = new
                {
                    r.RoomId,
                    r.Room_Number,
                    r.Room_Floor,
                    r.Room_Status,
                    r.Room_Rate,
                    r.Room_Description,
                    Room_Photo_1 = $"{Request.Scheme}://{Request.Host.Value}/{r.Room_Photo_1}",
                    Room_Photo_2 = $"{Request.Scheme}://{Request.Host.Value}/{r.Room_Photo_2}",
                    Room_Photo_3 = $"{Request.Scheme}://{Request.Host.Value}/{r.Room_Photo_3}",
                    r.Room_Type.Room_Type_Description,
                    r.Room_Type.Room_Type_Capacity,
                    r.Room_Type.Room_Size,
                };

                return Ok(rooms);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetRoomById/{roomId}")]
        public async Task<ActionResult> GetRoom(int roomId)
        {
            try
            {
                var r = await _repository.GetRoomByIdAsync(roomId);
                if (r == null) return NotFound("Room Does Not Exist");

                dynamic rooms = new
                {
                    r.RoomId,
                    r.Room_Number,
                    r.Room_Floor,
                    r.Room_Status,
                    r.Room_Rate,
                    r.Room_Description,
                    r.Room_Photo_1,
                    r.Room_Photo_2,
                    r.Room_Photo_3,
                    r.RoomTypeId,
                };

                return Ok(rooms);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRoom/{roomId}")]
        public async Task<ActionResult<RoomViewModel>> PutRoom(int roomId, [FromForm] RoomViewModel rvm)
        {
            try
            {
                var r = await _repository.GetRoomByIdAsync(roomId);
                if (r == null) return NotFound("Room Does Not Exist");

                r.Room_Number = rvm.Room_Number;
                r.Room_Floor = rvm.Room_Floor;
                r.Room_Status = rvm.Room_Status;
                r.Room_Rate = rvm.Room_Rate;
                r.Room_Description = rvm.Room_Description;
                r.RoomTypeId = rvm.RoomTypeId;
                if (rvm.Room_Photo_1 != null && rvm.Room_Photo_1.Length > 0)
                {
                    string fileName1 = ContentDispositionHeaderValue.Parse(rvm.Room_Photo_1.ContentDisposition).FileName.Trim('"');
                    string fullPath1 = Path.Combine(Directory.GetCurrentDirectory(), "roompics", fileName1);
                    using (var stream = new FileStream(fullPath1, FileMode.Create))
                    {
                        await rvm.Room_Photo_1.CopyToAsync(stream);
                    }
                    r.Room_Photo_1 = Path.Combine("roompics", fileName1);
                }

                if (rvm.Room_Photo_2 != null && rvm.Room_Photo_2.Length > 0)
                {
                    string fileName2 = ContentDispositionHeaderValue.Parse(rvm.Room_Photo_2.ContentDisposition).FileName.Trim('"');
                    string fullPath2 = Path.Combine(Directory.GetCurrentDirectory(), "roompics", fileName2);
                    using (var stream = new FileStream(fullPath2, FileMode.Create))
                    {
                        await rvm.Room_Photo_2.CopyToAsync(stream);
                    }
                    r.Room_Photo_2 = Path.Combine("roompics", fileName2);
                }

                if (rvm.Room_Photo_3 != null && rvm.Room_Photo_3.Length > 0)
                {
                    string fileName3 = ContentDispositionHeaderValue.Parse(rvm.Room_Photo_3.ContentDisposition).FileName.Trim('"');
                    string fullPath3 = Path.Combine(Directory.GetCurrentDirectory(), "roompics", fileName3);
                    using (var stream = new FileStream(fullPath3, FileMode.Create))
                    {
                        await rvm.Room_Photo_3.CopyToAsync(stream);
                    }
                    r.Room_Photo_3 = Path.Combine("roompics", fileName3);
                }

                await _repository.SaveChangesAsync();
                return Ok(r);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        //[HttpPost]
        //[Route("RoomPicUpload")]
        //[DisableRequestSizeLimit]
        //public IActionResult Upload()
        //{
        //    try
        //    {
        //        var file = Request.Form.Files[0];
        //        var folderName = Path.Combine("roompics");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if (file.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);
        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }
        //            return Ok(new { dbPath });
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        [HttpPost]
        [Route("AddRoom")]
        public async Task<IActionResult> PostRoom([FromForm] RoomViewModel rvm)
        {
            string folderName = "roompics";
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var room = new Room
            {
                Room_Number = rvm.Room_Number,
                Room_Floor = rvm.Room_Floor,
                Room_Status = "Available",
                Room_Rate = rvm.Room_Rate,
                Room_Description = rvm.Room_Description,
                RoomTypeId = rvm.RoomTypeId,
            };

            try
            {
                if (rvm.Room_Photo_1 != null && rvm.Room_Photo_1.Length > 0)
                {
                    string fileName1 = ContentDispositionHeaderValue.Parse(rvm.Room_Photo_1.ContentDisposition).FileName.Trim('"');
                    string fullPath1 = Path.Combine(pathToSave, fileName1);
                    using (var stream = new FileStream(fullPath1, FileMode.Create))
                    {
                        await rvm.Room_Photo_1.CopyToAsync(stream);
                    }
                    room.Room_Photo_1 = Path.Combine(folderName, fileName1).Replace("\\", "/");
                }

                if (rvm.Room_Photo_2 != null && rvm.Room_Photo_2.Length > 0)
                {
                    string fileName2 = ContentDispositionHeaderValue.Parse(rvm.Room_Photo_2.ContentDisposition).FileName.Trim('"');
                    string fullPath2 = Path.Combine(pathToSave, fileName2);
                    using (var stream = new FileStream(fullPath2, FileMode.Create))
                    {
                        await rvm.Room_Photo_2.CopyToAsync(stream);
                    }
                    room.Room_Photo_2 = Path.Combine(folderName, fileName2).Replace("\\", "/");
                }

                // Save Room_Photo_3
                if (rvm.Room_Photo_3 != null && rvm.Room_Photo_3.Length > 0)
                {
                    string fileName3 = ContentDispositionHeaderValue.Parse(rvm.Room_Photo_3.ContentDisposition).FileName.Trim('"');
                    string fullPath3 = Path.Combine(pathToSave, fileName3);
                    using (var stream = new FileStream(fullPath3, FileMode.Create))
                    {
                        await rvm.Room_Photo_3.CopyToAsync(stream);
                    }
                    room.Room_Photo_3 = Path.Combine(folderName, fileName3).Replace("\\", "/");
                }


                _repository.Add(room);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(room);
        }


        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
