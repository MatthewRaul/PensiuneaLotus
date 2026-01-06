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

        public IActionResult OnGet()
        {
            var guestList = _context.Guest
                .Select(g => new
                {
                    g.ID,
                    Display = (g.FirstName + " " + g.LastName) // adaptează la ce proprietăți ai în Guest
                })
                .ToList();

            var roomList = _context.Room
                .Select(r => new
                {
                    r.ID,
                    Display = $"{r.Number} | cap {r.Capacity} | {r.PricePerNight} lei/noapte" + (r.IsActive ? "" : " (inactiv)")
                })
                .ToList();

            ViewData["GuestID"] = new SelectList(guestList, "ID", "Display");
            ViewData["RoomID"] = new SelectList(roomList, "ID", "Display");

            return Page();
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // reumpli dropdown-urile și la invalid (altfel îți crapă pagina)
                var guestList = _context.Guest
                    .Select(g => new { g.ID, Display = (g.FirstName + " " + g.LastName) })
                    .ToList();

                var roomList = _context.Room
                    .Select(r => new
                    {
                        r.ID,
                        Display = $"{r.Number} | cap {r.Capacity} | {r.PricePerNight} lei/noapte" + (r.IsActive ? "" : " (inactiv)")
                    })
                    .ToList();

                ViewData["GuestID"] = new SelectList(guestList, "ID", "Display");
                ViewData["RoomID"] = new SelectList(roomList, "ID", "Display");

                return Page();
            }

            _context.Reservation.Add(Reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
