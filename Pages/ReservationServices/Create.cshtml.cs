using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Pages.ReservationServices
{
    public class CreateModel : PageModel
    {
        private readonly PensiuneaLotus.Data.PensiuneaLotusContext _context;

        public CreateModel(PensiuneaLotus.Data.PensiuneaLotusContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ReservationID"] = new SelectList(_context.Reservation, "ID", "ID");
        ViewData["ServiceID"] = new SelectList(_context.Service, "ID", "Name");
            return Page();
        }

        [BindProperty]
        public ReservationService ReservationService { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ReservationService.Add(ReservationService);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
