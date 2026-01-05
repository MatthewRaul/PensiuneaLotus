using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Models;

namespace PensiuneaLotus.Data
{
    public class PensiuneaLotusContext : DbContext
    {
        public PensiuneaLotusContext(DbContextOptions<PensiuneaLotusContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Room { get; set; } = default!;
        public DbSet<Guest> Guest { get; set; } = default!;
        public DbSet<Reservation> Reservation { get; set; } = default!;
        public DbSet<Payment> Payment { get; set; } = default!;
        public DbSet<Service> Service { get; set; } = default!;
        public DbSet<ReservationService> ReservationService { get; set; } = default!;
    }
}
