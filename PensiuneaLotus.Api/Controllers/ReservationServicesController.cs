using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationServicesController : ControllerBase
{
    private readonly PensiuneaLotusContext _context;

    public ReservationServicesController(PensiuneaLotusContext context)
    {
        _context = context;
    }

    // GET: api/ReservationServices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationService>>> GetReservationServices()
    {
        return await _context.ReservationService
            .Include(rs => rs.Reservation)
            .Include(rs => rs.Service)
            .ToListAsync();
    }

    // GET: api/ReservationServices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationService>> GetReservationService(int id)
    {
        var rs = await _context.ReservationService
            .Include(x => x.Reservation)
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.ID == id);

        if (rs == null) return NotFound();
        return rs;
    }

    // POST: api/ReservationServices
    [HttpPost]
    public async Task<ActionResult<ReservationService>> PostReservationService(ReservationService rs)
    {
        var reservationExists = await _context.Reservation.AnyAsync(r => r.ID == rs.ReservationID);
        if (!reservationExists) return BadRequest("ReservationID invalid.");

        var serviceExists = await _context.Service.AnyAsync(s => s.ID == rs.ServiceID);
        if (!serviceExists) return BadRequest("ServiceID invalid.");

        _context.ReservationService.Add(rs);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReservationService), new { id = rs.ID }, rs);
    }

    // PUT: api/ReservationServices/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReservationService(int id, ReservationService rs)
    {
        if (id != rs.ID) return BadRequest();

        var reservationExists = await _context.Reservation.AnyAsync(r => r.ID == rs.ReservationID);
        if (!reservationExists) return BadRequest("ReservationID invalid.");

        var serviceExists = await _context.Service.AnyAsync(s => s.ID == rs.ServiceID);
        if (!serviceExists) return BadRequest("ServiceID invalid.");

        _context.Entry(rs).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/ReservationServices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservationService(int id)
    {
        var rs = await _context.ReservationService.FindAsync(id);
        if (rs == null) return NotFound();

        _context.ReservationService.Remove(rs);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
