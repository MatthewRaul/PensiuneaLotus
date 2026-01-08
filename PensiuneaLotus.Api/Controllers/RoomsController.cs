using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly PensiuneaLotusContext _context;

        public RoomsController(PensiuneaLotusContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        // ACEASTA ESTE METODA MODIFICATA PENTRU AUTO-CURATARE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            // 1. LOGICA DE AUTO-ELIBERARE
            // Cautam toate camerele care sunt marcate ca 'Ocupat'
            var occupiedRooms = await _context.Room
                                      .Where(r => r.IsOccupied == true)
                                      .ToListAsync();

            bool changesMade = false;
            DateTime now = DateTime.Now;

            foreach (var room in occupiedRooms)
            {
                // Verificam daca exista vreo rezervare ACTIVA pentru aceasta camera
                // (Adica o rezervare unde CheckOut-ul este in viitor)
                // Nota: Daca ai eroare la '.Reservation', incearca '.Reservations'
                bool hasActiveReservation = await _context.Reservation
                    .AnyAsync(res => res.RoomID == room.ID && res.CheckOutDate > now);

                // Daca NU exista nicio rezervare activa (toate au expirat), eliberam camera
                if (!hasActiveReservation)
                {
                    room.IsOccupied = false; // O facem verde (libera)
                    changesMade = true;
                }
            }

            // Salvam modificarile in baza de date doar daca am schimbat ceva
            if (changesMade)
            {
                await _context.SaveChangesAsync();
            }

            // 2. RETURNAM LISTA ACTUALIZATA (Corecta)
            return await _context.Room.ToListAsync();
        }

        // GET: api/Rooms/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAvailableRooms()
        {
            return await _context.Room
                .Where(r => !r.IsOccupied)
                .ToListAsync();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Room.FindAsync(id);

            if (room == null)
                return NotFound();

            return room;
        }

        // POST: api/Rooms
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            _context.Room.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.ID }, room);
        }

        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.ID)
                return BadRequest();

            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Room.FindAsync(id);
            if (room == null)
                return NotFound();

            _context.Room.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}