using System.Threading.Tasks;
using DeskBooker.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeskBooker.Data.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<DeskBooking> DeskBookings { get; set; }
    }
}
