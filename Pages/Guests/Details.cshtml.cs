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
    public class DetailsModel : PageModel
    {
        private readonly PensiuneaLotus.Data.PensiuneaLotusContext _context;

        public DetailsModel(PensiuneaLotus.Data.PensiuneaLotusContext context)
        {
            _context = context;
        }

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
    }
}
