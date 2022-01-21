using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.API.Models;
using ProjetArchiLog.Library.Data;

namespace ProjetArchiLog.API.Data
{
    public class ArchiDbContext : BaseDbContext
    {
        public ArchiDbContext(DbContextOptions options):base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
