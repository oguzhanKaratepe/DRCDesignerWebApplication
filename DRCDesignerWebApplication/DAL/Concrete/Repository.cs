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
        private DbSet<TEntity> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();

        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public IEnumerable<TEntity> GetAll()
        {
           return  _dbSet.ToList();
        }

        public TEntity GetById(int id)
        {
           return _dbSet.Find(id);
        }

        public void remove(int id)
        {
            _dbSet.Remove(GetById(id));
        }

        public void remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
