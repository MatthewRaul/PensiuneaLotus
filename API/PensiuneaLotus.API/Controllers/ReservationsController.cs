using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly PensiuneaLotusContext _context;

        public ReservationsController(PensiuneaLotusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (reservation == null) return NotFound();
            return reservation;
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            // verifică FK-uri
            var roomExists = await _context.Room.AnyAsync(r => r.ID == reservation.RoomID);
            var guestExists = await _context.Guest.AnyAsync(g => g.ID == reservation.GuestID);

            if (!roomExists) return BadRequest("RoomID invalid.");
            if (!guestExists) return BadRequest("GuestID invalid.");

            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.ID }, reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.ID) return BadRequest();

            var roomExists = await _context.Room.AnyAsync(r => r.ID == reservation.RoomID);
            var guestExists = await _context.Guest.AnyAsync(g => g.ID == reservation.GuestID);

            if (!roomExists) return BadRequest("RoomID invalid.");
            if (!guestExists) return BadRequest("GuestID invalid.");

            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null) return NotFound();

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}