using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Pages.ReservationServices
{
    public class EditModel : PageModel
    {
        private readonly PensiuneaLotus.Data.PensiuneaLotusContext _context;

        public EditModel(PensiuneaLotus.Data.PensiuneaLotusContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ReservationService ReservationService { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationservice =  await _context.ReservationService.FirstOrDefaultAsync(m => m.ID == id);
            if (reservationservice == null)
            {
                return NotFound();
            }
            ReservationService = reservationservice;
           ViewData["ReservationID"] = new SelectList(_context.Reservation, "ID", "ID");
           ViewData["ServiceID"] = new SelectList(_context.Service, "ID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ReservationService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationServiceExists(ReservationService.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReservationServiceExists(int id)
        {
            return _context.ReservationService.Any(e => e.ID == id);
        }
    }
}
