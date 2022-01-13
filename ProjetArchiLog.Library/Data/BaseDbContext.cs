using Microsoft.EntityFrameworkCore;

namespace ProjetArchiLog.Library.Data
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            ChangeDeletedState();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeDeletedState();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ChangeDeletedState()
        {
            var delEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
            foreach (var item in delEntities)
            {
                if (item.Entity is BaseModel model)
                {
                    model.Active = false;
                    model.DeletedAt = DateTime.Now;
                    item.State = EntityState.Modified;
                }
            }
        }
    }
}
