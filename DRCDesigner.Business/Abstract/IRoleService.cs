using DRCDesigner.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DRCDesigner.Business.Abstract
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAll();
        void Add(string values);
        void Update(string values, int id);
        bool Remove(int roleId);
    }
}
