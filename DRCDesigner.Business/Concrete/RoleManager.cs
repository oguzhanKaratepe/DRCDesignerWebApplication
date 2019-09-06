using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.Concrete
{
    public class RoleManager:IRoleService
    {
        private IRoleUnitOfWork _roleUnitOfWork;
        public RoleManager(IRoleUnitOfWork roleUnitOfWork)
        {
            _roleUnitOfWork = roleUnitOfWork;
        }

        public void Add(string values)
        {
            var newRole = new Role();
            JsonConvert.PopulateObject(values, newRole);
            _roleUnitOfWork.RoleRepository.Add(newRole);
            _roleUnitOfWork.Complete();
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _roleUnitOfWork.RoleRepository.GetAll();
        }
        public void Update(string values, int id)
        {
            var role = _roleUnitOfWork.RoleRepository.GetById(id);
            JsonConvert.PopulateObject(values, role);
           _roleUnitOfWork.RoleRepository.Update(role);
           _roleUnitOfWork.Complete();
        }
        public bool Remove(int roleId)
        {
            if (roleId > 0)
            {
                _roleUnitOfWork.RoleRepository.Remove(roleId);
            }
            else
            {
                return false;
            }

            return true;
        }

      
    }
}
