using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly PensiuneaLotusContext _context;

    public PaymentsController(PensiuneaLotusContext context)
    {
        _context = context;
    }

    // GET: api/Payments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
    {
        return await _context.Payment
            .Include(p => p.Reservation)
            .ToListAsync();
    }

    // GET: api/Payments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPayment(int id)
    {
        var payment = await _context.Payment
            .Include(p => p.Reservation)
            .FirstOrDefaultAsync(p => p.ID == id);

        if (payment == null) return NotFound();
        return payment;
    }

    // POST: api/Payments
    [HttpPost]
    public async Task<ActionResult<Payment>> PostPayment(Payment payment)
    {
        var reservationExists = await _context.Reservation.AnyAsync(r => r.ID == payment.ReservationID);
        if (!reservationExists) return BadRequest("ReservationID invalid.");

        _context.Payment.Add(payment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPayment), new { id = payment.ID }, payment);
    }

    // PUT: api/Payments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPayment(int id, Payment payment)
    {
        if (id != payment.ID) return BadRequest();

        var reservationExists = await _context.Reservation.AnyAsync(r => r.ID == payment.ReservationID);
        if (!reservationExists) return BadRequest("ReservationID invalid.");

        _context.Entry(payment).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Payments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var payment = await _context.Payment.FindAsync(id);
        if (payment == null) return NotFound();

        _context.Payment.Remove(payment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
