using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL
{
     public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _context;
        
        public Repository(DbContext context)
        {
            _context = context;

        }
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
           return await _context.Set<TEntity>().ToListAsync();
        }

        public TEntity GetById(int id)
        {
           return _context.Set<TEntity>().Find(id);
        }

        public void Remove(int id)
        {
            _context.Set<TEntity>().Remove(GetById(id));
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
