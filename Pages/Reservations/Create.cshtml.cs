using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly PensiuneaLotusContext _context;

        public CreateModel(PensiuneaLotusContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        private void PopulateDropDowns(int? selectedGuestId = null, int? selectedRoomId = null)
        {
            var guestList = _context.Guest
                .AsNoTracking()
                .Select(g => new { g.ID, Display = g.FirstName + " " + g.LastName })
                .ToList();

            // Doar camere LIBERE (IsOccupied=false)
            var roomList = _context.Room
                .Where(r => !r.IsOccupied)
                .Select(r => new
    {
        r.ID,
        Display = $"{r.Number} | cap {r.Capacity} | {r.PricePerNight} lei/noapte"
    })
    .ToList();


            ViewData["GuestID"] = new SelectList(guestList, "ID", "Display", selectedGuestId);
            ViewData["RoomID"] = new SelectList(roomList, "ID", "Display", selectedRoomId);
        }

        public IActionResult OnGet()
        {
            PopulateDropDowns();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // reumpli dropdown-urile ca în codul tău existent
                return Page();
            }

            var room = await _context.Room.FirstOrDefaultAsync(r => r.ID == Reservation.RoomID);
            if (room == null)
            {
                ModelState.AddModelError(string.Empty, "Camera invalidă.");
                return Page();
            }

            if (room.IsOccupied)
            {
                ModelState.AddModelError(string.Empty, "Camera este deja ocupată.");
                return Page();
            }

            _context.Reservation.Add(Reservation);
            room.IsOccupied = true;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");

        }

    }
}
