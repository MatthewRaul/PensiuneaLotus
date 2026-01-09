using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PensiuneaLotus.Data; // Asigura-te ca ai using-ul corect pentru Context
using System.Linq;

namespace PensiuneaLotus.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PensiuneaLotusContext _context; // 1. Adaugam contextul bazei de date

        // Constructorul primeste acum si Contextul
        public IndexModel(ILogger<IndexModel> logger, PensiuneaLotusContext context)
        {
            _logger = logger;
            _context = context;
        }

        // 2. Proprietatea care va tine suma
        public decimal TotalIncasari { get; set; } = 0;

        public void OnGet()
        {
            // 3. Calculam suma tuturor platilor din baza de date
            // Folosim Sum(p => p.Amount)
            if (_context.Payment != null && _context.Payment.Any())
            {
                TotalIncasari = _context.Payment.Sum(p => p.Amount);
            }
        }
    }
}