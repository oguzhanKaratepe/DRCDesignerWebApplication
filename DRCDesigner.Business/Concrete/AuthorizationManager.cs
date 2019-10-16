using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.Concrete
{
    public class AuthorizationManager : IAuthorizationService
    {
        private IMapper _mapper;
        private IRoleUnitOfWork _roleUnitOfWork;
        public AuthorizationManager(IRoleUnitOfWork roleUnitOfWork, IMapper mapper)
        {
            _roleUnitOfWork = roleUnitOfWork;
            _mapper = mapper;
        }
        public async Task<IList<AuthorizationBusinessModel>> GetCardAuthorizations(int cardId)
        {
            IEnumerable<Authorization> authorizations = await _roleUnitOfWork.AuthorizationRepository.GetAuthorizationsByDrcCardId(cardId);

            var authorizationModels = new List<AuthorizationBusinessModel>();

            foreach (var authorization in authorizations)
            {
                var authorizationModel = _mapper.Map<AuthorizationBusinessModel>(authorization);
                int i = 0;
                int[] tempIds = new int[authorization.AuthorizationRoles.Count];
                foreach (var autRole in authorization.AuthorizationRoles)
                {
                    authorizationModel.Roles.Add(_roleUnitOfWork.RoleRepository.GetById(autRole.RoleId));
                    tempIds[i] = autRole.RoleId;
                    i++;
                }

                authorizationModel.RoleIds = tempIds;
                authorizationModels.Add(authorizationModel);
            }

            return authorizationModels;
        }
        public void Add(string values)
        {
            var newAuthorizationModel = new AuthorizationBusinessModel();
            JsonConvert.PopulateObject(values, newAuthorizationModel);

            
            var newAuthorization = _mapper.Map<Authorization>(newAuthorizationModel);
            AuthorizationRole newAutRole;
            foreach (var roleId in newAuthorizationModel.RoleIds)
            {
                newAutRole = new AuthorizationRole();
                newAutRole.Authorization = newAuthorization;
                newAutRole.Role = _roleUnitOfWork.RoleRepository.GetById(roleId);
                newAuthorization.AuthorizationRoles.Add(newAutRole);
            }

            _roleUnitOfWork.AuthorizationRepository.Add(newAuthorization);
            _roleUnitOfWork.Complete();
        }

        public void Update(int id, string values)
        {
            Authorization authorization = _roleUnitOfWork.AuthorizationRepository.GetById(id);
            AuthorizationBusinessModel authorizationBusinessModel = _mapper.Map<AuthorizationBusinessModel>(authorization);
            _roleUnitOfWork.AuthorizationRepository.Remove(authorization);
            var autRoleCollections = _roleUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(id);

            int[] RoleIds = new int[autRoleCollections.Count];
            int i = 0;
            foreach (var autRoleCollection in autRoleCollections)
            {
                RoleIds[i] = autRoleCollection.RoleId;
                _roleUnitOfWork.AuthorizationRoleRepository.Remove(autRoleCollection);
                i++;
            }
            authorizationBusinessModel.RoleIds = RoleIds;
            JsonConvert.PopulateObject(values, authorizationBusinessModel);

            var newAuthorization = _mapper.Map<Authorization>(authorizationBusinessModel);
            if (authorizationBusinessModel.RoleIds != null)
            {
                AuthorizationRole autRole;
                foreach (var autRoleId in authorizationBusinessModel.RoleIds)
                {
                    autRole = new AuthorizationRole();
                    autRole.Role = _roleUnitOfWork.RoleRepository.GetById(autRoleId);
                    autRole.Authorization = newAuthorization;
                    newAuthorization.AuthorizationRoles.Add(autRole);
                }
            }

            _roleUnitOfWork.AuthorizationRepository.Add(newAuthorization);
            _roleUnitOfWork.Complete();
        }
        public void Delete(int id)
        {
            var autRoleConnections =
                _roleUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(id);
            foreach (var autRole in autRoleConnections)
            {
                _roleUnitOfWork.AuthorizationRoleRepository.Remove(autRole);
            }
            _roleUnitOfWork.AuthorizationRepository.Remove(id);
            _roleUnitOfWork.Complete();
        }

        public async Task<IEnumerable<object>> GetAuthorizationRoles(int id)
        {
            //this is roles with version and global ones
            var subdomainVersionRoles = await _roleUnitOfWork.SubdomainVersionRoleRepository.GetAllVersionRolesBySubdomainVersionId(id);
         
            IList<RoleBusinessModel> rolesBag = new List<RoleBusinessModel>();

            foreach (var subdomainVersionRole in subdomainVersionRoles)
            {
                var businessRoleModel =
                    _mapper.Map<RoleBusinessModel>(_roleUnitOfWork.RoleRepository.GetById(subdomainVersionRole.RoleId));
              rolesBag.Add(businessRoleModel);

            }

            var globalRoles = await _roleUnitOfWork.RoleRepository.getGlobalRoles();
            foreach (var globalRole in globalRoles)
            {
                var businessRoleModel = _mapper.Map<RoleBusinessModel>(globalRole);
                rolesBag.Add(businessRoleModel);

            }
            return rolesBag;
        }
    }

}
