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
    public class IndexModel : PageModel
    {
        private readonly PensiuneaLotus.Data.PensiuneaLotusContext _context;

        public IndexModel(PensiuneaLotus.Data.PensiuneaLotusContext context)
        {
            _context = context;
        }

        public IList<ReservationService> ReservationService { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ReservationService = await _context.ReservationService
                .Include(r => r.Reservation)
                .Include(r => r.Service).ToListAsync();
        }
    }
}
