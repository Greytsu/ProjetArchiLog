using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.Library.Data;

namespace ProjetArchiLog.API.Data
{
    public class ArchiDbContext : BaseDbContext
    {
        public ArchiDbContext(DbContextOptions options):base(options)
        {
        }
    }
}
