using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL
{
    public interface IRepository<TEntity> where TEntity: class
    {
        void Add(TEntity entity);
        void remove(int id);
        void remove(TEntity entity);
        void update(TEntity entity);

        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
    }
}
