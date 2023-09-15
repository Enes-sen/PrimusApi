using Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using TContext context = new();
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Added;
            _ = context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            using TContext context = new();
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            _ = context.SaveChanges();
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            using TContext context = new();
            return context.Set<TEntity>().FirstOrDefault(filter);
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>>? filter = null)
        {
            using TContext context = new();
            return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
        }

        public void Update(TEntity entity)
        {
            using TContext context = new();
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> updatedEntity = context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _ = context.SaveChanges();
        }
    }
}
