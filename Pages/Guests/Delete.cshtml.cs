using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Pages.Guests
{
    public class DeleteModel : PageModel
    {
        private readonly PensiuneaLotus.Data.PensiuneaLotusContext _context;

        public DeleteModel(PensiuneaLotus.Data.PensiuneaLotusContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guest Guest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guest.FirstOrDefaultAsync(m => m.ID == id);

            if (guest == null)
            {
                return NotFound();
            }
            else
            {
                Guest = guest;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var guest = await _context.Guest.FindAsync(id.Value);
            if (guest == null) return NotFound();

            var reservations = await _context.Reservation
                .Where(r => r.GuestID == id.Value)
                .ToListAsync();

            var roomIds = reservations.Select(r => r.RoomID).Distinct().ToList();
            var rooms = await _context.Room.Where(r => roomIds.Contains(r.ID)).ToListAsync();

            foreach (var room in rooms)
                room.IsOccupied = false;

            if (reservations.Count > 0)
                _context.Reservation.RemoveRange(reservations);

            _context.Guest.Remove(guest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}
