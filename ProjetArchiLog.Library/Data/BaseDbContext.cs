using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.Library.Models;

namespace ProjetArchiLog.Library.Data
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            ChangeCreateState();
            ChangeUpdateState();
            ChangeDeletedState();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeCreateState();
            ChangeUpdateState();
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
                    model.IsDeleted = true;
                    model.DeletedAt = DateTime.Now;
                    item.State = EntityState.Modified;
                }
            }
        }

        private void ChangeCreateState()
        {
            var createEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
            foreach (var item in createEntities)
            {
                if (item.Entity is BaseModel model)
                {
                    model.IsDeleted = false;
                    model.DeletedAt = null;
                    model.CreatedAt = DateTime.Now;
                    model.UpdatedAt = DateTime.Now;
                    item.State = EntityState.Added;
                }
            }
        }

        private void ChangeUpdateState()
        {
            var updateEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
            foreach (var item in updateEntities)
            {
                if (item.Entity is BaseModel model)
                {
                    model.UpdatedAt = DateTime.Now;
                    item.State = EntityState.Modified;
                }
            }
        }
    }
}
