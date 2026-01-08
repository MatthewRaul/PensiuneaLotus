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
                .AsNoTracking()
                .Select(g => new { g.ID, Display = g.FirstName + " " + g.LastName })
                .ToList();

            // camere libere + camera curent selectată (ca să apară în dropdown chiar dacă e ocupată)
            var roomList = _context.Room
                .AsNoTracking()
                .Where(r => !r.IsOccupied || r.ID == selectedRoomId)
                .Select(r => new
                {
                    r.ID,
                    Display = $"{r.Number} | cap {r.Capacity} | {r.PricePerNight} lei/noapte" +
                              (r.IsOccupied ? " (ocupată)" : " (liberă)")
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
            if (Reservation.CheckOutDate <= Reservation.CheckInDate)
                ModelState.AddModelError(nameof(Reservation.CheckOutDate), "Check-out trebuie să fie după check-in.");

            if (!ModelState.IsValid)
            {
                PopulateDropDowns(Reservation.GuestID, Reservation.RoomID);
                return Page();
            }

            // rezervarea originală (ca să știm camera veche)
            var existing = await _context.Reservation.AsNoTracking().FirstOrDefaultAsync(r => r.ID == Reservation.ID);
            if (existing == null) return NotFound();

            // dacă s-a schimbat camera
            if (existing.RoomID != Reservation.RoomID)
            {
                var oldRoom = await _context.Room.FirstOrDefaultAsync(r => r.ID == existing.RoomID);
                if (oldRoom != null) oldRoom.IsOccupied = false; // eliberează

                var newRoom = await _context.Room.FirstOrDefaultAsync(r => r.ID == Reservation.RoomID);
                if (newRoom == null)
                {
                    ModelState.AddModelError(nameof(Reservation.RoomID), "Camera selectată nu există.");
                    PopulateDropDowns(Reservation.GuestID, Reservation.RoomID);
                    return Page();
                }

                if (newRoom.IsOccupied)
                {
                    ModelState.AddModelError(nameof(Reservation.RoomID), "Camera selectată este deja ocupată.");
                    PopulateDropDowns(Reservation.GuestID, Reservation.RoomID);
                    return Page();
                }

                newRoom.IsOccupied = true; // ocupă noua cameră
            }

            _context.Attach(Reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
