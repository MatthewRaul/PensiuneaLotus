using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Pages.ReservationServices
{
    public class DetailsModel : PageModel
    {
        private readonly PensiuneaLotus.Data.PensiuneaLotusContext _context;

        public DetailsModel(PensiuneaLotus.Data.PensiuneaLotusContext context)
        {
            _context = context;
        }

        public ReservationService ReservationService { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationservice = await _context.ReservationService.FirstOrDefaultAsync(m => m.ID == id);
            if (reservationservice == null)
            {
                return NotFound();
            }
            else
            {
                ReservationService = reservationservice;
            }
            return Page();
        }
    }
}
