using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL
{
    public interface IRepository<TEntity> where TEntity: class
    {



        void Add(TEntity entity);
        void Remove(int id);
        void Remove(TEntity entity);
        void Update(TEntity entity);

        TEntity GetById(int id);
        Task <IEnumerable<TEntity>> GetAll();
    }
}
