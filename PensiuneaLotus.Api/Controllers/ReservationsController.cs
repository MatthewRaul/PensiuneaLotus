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

        // GET: api/Rooms/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAvailableRooms()
        {
            return await _context.Room
                .Where(r => !r.IsOccupied)
                .ToListAsync();
        }


        // POST: api/Reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            var room = await _context.Room.FindAsync(reservation.RoomID);
            if (room == null) return BadRequest("RoomID invalid.");

            var guestExists = await _context.Guest.AnyAsync(g => g.ID == reservation.GuestID);
            if (!guestExists) return BadRequest("GuestID invalid.");

            if (room.IsOccupied) return Conflict("Camera este deja ocupată.");

            _context.Reservation.Add(reservation);
            room.IsOccupied = true;

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.ID }, reservation);
        }




        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.ID) return BadRequest();

            if (reservation.CheckOutDate <= reservation.CheckInDate)
                return BadRequest("Check-out trebuie să fie după check-in.");

            var existing = await _context.Reservation
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ID == id);

            if (existing == null) return NotFound();

            var guestExists = await _context.Guest.AnyAsync(g => g.ID == reservation.GuestID);
            if (!guestExists) return BadRequest("GuestID invalid.");

            if (existing.RoomID != reservation.RoomID)
            {
                var newRoom = await _context.Room.FindAsync(reservation.RoomID);
                if (newRoom == null) return BadRequest("RoomID invalid.");
                if (newRoom.IsOccupied) return Conflict("Camera este deja ocupată.");

                var oldRoom = await _context.Room.FindAsync(existing.RoomID);
                if (oldRoom != null) oldRoom.IsOccupied = false;

                newRoom.IsOccupied = true;
            }

            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/Reservations/5

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null) return NotFound();

            var room = await _context.Room.FindAsync(reservation.RoomID);
            if (room != null) room.IsOccupied = false;

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }





    }
}