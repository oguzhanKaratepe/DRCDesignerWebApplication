using DRCDesigner.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;


namespace DRCDesigner.Business.Abstract
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleBusinessModel>> GetAll();
        void Add(string values);
        void Update(string values, int id);
        Task<bool> Remove(int roleId);
    }
}
