using System;
using System.Collections.Generic;
using System.Text;
using DRCDesigner.Business.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Concrete;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Concrete
{
    public class InitialDataManager:InitialDataService
    { 

        private ISubdomainUnitOfWork _subdomainUnitOfWork;
        public InitialDataManager(ISubdomainUnitOfWork subdomainUnitOfWork)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;
        }
   
        public void addGlobalRoles()
        {
            var role = new Role();
            role.RoleName = "Admin";
            _subdomainUnitOfWork.RoleRepository.Add(role);
        }
    }
}
