using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private IRoleUnitOfWork _roleUnitOfWork;
        private IMapper _mapper;

        public RoleManager(IRoleUnitOfWork roleUnitOfWork, IMapper mapper)
        {
            _roleUnitOfWork = roleUnitOfWork;
            _mapper = mapper;
        }

        public void Add(string values)
        {
            RoleBusinessModel roleViewModel = new RoleBusinessModel();
            JsonConvert.PopulateObject(values, roleViewModel);
            var newRole = _mapper.Map<Role>(roleViewModel);

            if (roleViewModel.SubdomainVersionRoleIds != null)
            {
                foreach (var SubdomainVersionId in roleViewModel.SubdomainVersionRoleIds)
                {
                    SubdomainVersionRole subdomainVersionRole = new SubdomainVersionRole();
                    subdomainVersionRole.SubdomainVersionId = SubdomainVersionId;
                    subdomainVersionRole.Role = newRole;
                    newRole.SubdomainVersionRoles.Add(subdomainVersionRole);
                }
            }

            _roleUnitOfWork.RoleRepository.Add(newRole);
            _roleUnitOfWork.Complete();
        }

        public async Task<IEnumerable<RoleBusinessModel>> GetAllSubdomainRoles(int subdomainId)
        {
            var subdomainWithVersions = await _roleUnitOfWork.SubdomainRepository.GetSubdomainWithAllVersions(subdomainId);

            IList<RoleBusinessModel> roleBusinessModels = new List<RoleBusinessModel>();
            List<int> selectedRoleIds=new List<int>();
            foreach (var subdomainVersion in  subdomainWithVersions.SubdomainVersions)
            {
                var SubdomainVersionRoles =  _roleUnitOfWork.SubdomainVersionRoleRepository.GetAllVersionRolesBySubdomainVersionId(subdomainVersion.Id);

                foreach (var SubdomainVersionRole in SubdomainVersionRoles)
                {
                    var role = _roleUnitOfWork.RoleRepository.GetById(SubdomainVersionRole.RoleId);

                    if (!selectedRoleIds.Contains(role.Id)) { 

                    var roleBusinessModel = _mapper.Map<RoleBusinessModel>(role);
                    var roleVersions =  _roleUnitOfWork.SubdomainVersionRoleRepository.GetAllRoleVersionsByRoleId(SubdomainVersionRole.RoleId);

                    roleBusinessModel.SubdomainVersionRoleIds = new int[roleVersions.Count()];
                    int i = 0;
                    foreach (var roleVersion in roleVersions)
                    {
                        var roleVersionName = _roleUnitOfWork.SubdomainVersionRepository.getVersionNumber((int)roleVersion.SubdomainVersionId);
                            roleBusinessModel.RoleVersionNumbers += roleVersionName +" ";
                        roleBusinessModel.SubdomainVersionRoleIds[i] =(int)roleVersion.SubdomainVersionId;
                        i++;
                    }
                    roleBusinessModels.Add(roleBusinessModel);
                    selectedRoleIds.Add(role.Id);
                    }
                }

            }


            return roleBusinessModels;

        }

        public async Task Update(string values, int id)
        {
            var role = _roleUnitOfWork.RoleRepository.GetById(id);

            var roleSubdomainVersions =
                _roleUnitOfWork.SubdomainVersionRoleRepository.GetAllRoleVersionsByRoleId(id);
            JsonConvert.PopulateObject(values, role);

            RoleBusinessModel roleBusinessModel = new RoleBusinessModel();

            roleBusinessModel.SubdomainVersionRoleIds = new int[roleSubdomainVersions.Count()];
            int i = 0;
            foreach (var roleSubdomainVersion in roleSubdomainVersions)
            {
                roleBusinessModel.SubdomainVersionRoleIds[i] =(int) roleSubdomainVersion.SubdomainVersionId;
                _roleUnitOfWork.SubdomainVersionRoleRepository.Remove(roleSubdomainVersion);
                i++;
            }

            JsonConvert.PopulateObject(values, roleBusinessModel);

            foreach (var latestRoleVersionId in roleBusinessModel.SubdomainVersionRoleIds)
            {
                var newSubdomainVersionRole = new SubdomainVersionRole();
                newSubdomainVersionRole.SubdomainVersionId = latestRoleVersionId;
                newSubdomainVersionRole.Role = role;
                role.SubdomainVersionRoles.Add(newSubdomainVersionRole);
            }

            _roleUnitOfWork.Complete();
        }

        public async Task<bool> Remove(int roleId)
        {
            if (roleId > 0)
            {
                _roleUnitOfWork.RoleRepository.Remove(roleId);
               var subdomainVersionRoles= _roleUnitOfWork.SubdomainVersionRoleRepository.GetAllRoleVersionsByRoleId(roleId);

               foreach (var subdomainVersionRole in subdomainVersionRoles)
               {
                   _roleUnitOfWork.SubdomainVersionRoleRepository.Remove(subdomainVersionRole);
               }

               var authorizationRoles =
                   _roleUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByRoleId(roleId);
               foreach (var authorizationRole in authorizationRoles)
               {
                  _roleUnitOfWork.AuthorizationRoleRepository.Remove(authorizationRole); 
               }

                _roleUnitOfWork.Complete();

               return true;
            }
            
            return false;

        }


    }
}
