using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.Library.Data;
using ProjetArchiLog.API.Models;

namespace ProjetArchiLog.API.Data
{
    public class ArchiDbContext : BaseDbContext
    {
        public ArchiDbContext(DbContextOptions options):base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
    }
}
