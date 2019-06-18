using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL
{
     public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        public Repository()
        {

        }
        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void remove(int id)
        {
            throw new NotImplementedException();
        }

        public void remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
