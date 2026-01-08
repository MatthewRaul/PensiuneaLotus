using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Pages.Reservations
{
    public class DeleteModel : PageModel
    {
        private readonly PensiuneaLotusContext _context;

        public DeleteModel(PensiuneaLotusContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservation.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            if (reservation == null) return NotFound();

            Reservation = reservation;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservation.FindAsync(id.Value);
            if (reservation == null) return NotFound();

            var room = await _context.Room.FindAsync(reservation.RoomID);
            if (room != null) room.IsOccupied = false;

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}
