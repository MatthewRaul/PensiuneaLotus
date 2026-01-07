using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Pages.Reservations
{
    public class EditModel : PageModel
    {
        private readonly PensiuneaLotusContext _context;

        public EditModel(PensiuneaLotusContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        private void PopulateDropDowns(int? selectedGuestId = null, int? selectedRoomId = null)
        {
            var guestList = _context.Guest
                .Select(g => new
                {
                    g.ID,
                    Display = g.FirstName + " " + g.LastName
                })
                .ToList();

            var roomList = _context.Room
                .Select(r => new
                {
                    r.ID,
                    Display = $"{r.Number} | cap {r.Capacity} | {r.PricePerNight} lei/noapte" +
                              (r.IsActive ? "" : " (inactiv)")
                })
                .ToList();

            ViewData["GuestID"] = new SelectList(guestList, "ID", "Display", selectedGuestId);
            ViewData["RoomID"] = new SelectList(roomList, "ID", "Display", selectedRoomId);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Reservation = await _context.Reservation
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ID == id);

            if (Reservation == null) return NotFound();

            PopulateDropDowns(Reservation.GuestID, Reservation.RoomID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // exemplu de validare extra (opțional)
            if (Reservation.CheckOutDate < Reservation.CheckInDate)
            {
                ModelState.AddModelError("Reservation.CheckOutDate",
                    "Check-out trebuie să fie după check-in.");
            }

            if (!ModelState.IsValid)
            {
                PopulateDropDowns(Reservation.GuestID, Reservation.RoomID);
                return Page();
            }

            // Asta e partea crucială: EDIT = update, nu Add
            _context.Attach(Reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.Reservation.AnyAsync(r => r.ID == Reservation.ID);
                if (!exists) return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}