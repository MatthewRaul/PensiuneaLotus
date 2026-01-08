using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly PensiuneaLotusContext _context;

        public GuestsController(PensiuneaLotusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuests()
            => await _context.Guest.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuest(int id)
        {
            var guest = await _context.Guest.FindAsync(id);
            if (guest == null) return NotFound();
            return guest;
        }

        [HttpPost]
        public async Task<ActionResult<Guest>> PostGuest(Guest guest)
        {
            _context.Guest.Add(guest);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGuest), new { id = guest.ID }, guest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(int id, Guest guest)
        {
            if (id != guest.ID) return BadRequest();

            _context.Entry(guest).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var guest = await _context.Guest.FindAsync(id);
            if (guest == null) return NotFound();

            var reservations = await _context.Reservation
                .Where(r => r.GuestID == id)
                .ToListAsync();

            var roomIds = reservations.Select(r => r.RoomID).Distinct().ToList();
            var rooms = await _context.Room.Where(r => roomIds.Contains(r.ID)).ToListAsync();

            foreach (var room in rooms)
                room.IsOccupied = false;

            if (reservations.Count > 0)
                _context.Reservation.RemoveRange(reservations);

            _context.Guest.Remove(guest);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}

