using KompaniaPchor.ORM_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Repositories
{
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : class
    {
        private readonly PchorContext _context;

        public GenericRepo(PchorContext context)
        {
            _context = context;
        }
        public virtual IQueryable<TEntity> Get()
        {
            return _context.Set<TEntity>();
        }

        public virtual TEntity Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return entity;
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
