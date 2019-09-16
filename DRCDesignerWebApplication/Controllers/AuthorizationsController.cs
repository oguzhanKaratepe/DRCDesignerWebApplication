using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using DRCDesignerWebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DRCDesignerWebApplication.Controllers
{
    public class AuthorizationsController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private IMapper _mapper;
        private IRoleUnitOfWork _roleUnitOfWork;
        public AuthorizationsController(IAuthorizationService authorizationService,IRoleUnitOfWork roleUnitOfWork, IMapper mapper)
        {
            _authorizationService = authorizationService;
            _roleUnitOfWork = roleUnitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<object> Get(int Id, DataSourceLoadOptions loadOptions)
        {

            IEnumerable<Authorization> authorizations = await _roleUnitOfWork.AuthorizationRepository.GetAuthorizationsByDrcCardId(Id);

            var authorizationModels = new List<AuthorizationViewModel>();

            foreach (var authorization in authorizations)
            {
                var authorizationModel = _mapper.Map<AuthorizationViewModel>(authorization);
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

            return DataSourceLoader.Load(authorizationModels,loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var newAuthorizationModel = new AuthorizationViewModel();
            JsonConvert.PopulateObject(values, newAuthorizationModel);

            if (!TryValidateModel(newAuthorizationModel))
                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"

            var newAuthorization = _mapper.Map<Authorization>(newAuthorizationModel);
            AuthorizationRole newAutRole;
            foreach (var roleId in newAuthorizationModel.RoleIds)
            {
                newAutRole=new AuthorizationRole();
                newAutRole.Authorization = newAuthorization;
                newAutRole.Role = _roleUnitOfWork.RoleRepository.GetById(roleId);
                newAuthorization.AuthorizationRoles.Add(newAutRole);
            }

            _roleUnitOfWork.AuthorizationRepository.Add(newAuthorization);
            _roleUnitOfWork.Complete();
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            Authorization authorization = _roleUnitOfWork.AuthorizationRepository.GetById(key);
            AuthorizationViewModel authorizationViewModel = _mapper.Map<AuthorizationViewModel>(authorization);
            _roleUnitOfWork.AuthorizationRepository.Remove(authorization);
            var autRoleCollections = _roleUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(key);

            int[] RoleIds = new int[autRoleCollections.Count];
            int i = 0;
            foreach (var autRoleCollection in autRoleCollections)
            {
                RoleIds[i] = autRoleCollection.RoleId;
                _roleUnitOfWork.AuthorizationRoleRepository.Remove(autRoleCollection);
                i++;
            }
            authorizationViewModel.RoleIds = RoleIds;
            JsonConvert.PopulateObject(values, authorizationViewModel);

            var newAuthorization = _mapper.Map<Authorization>(authorizationViewModel);
            if (authorizationViewModel.RoleIds != null)
            {
                AuthorizationRole autRole;
                foreach (var autRoleId in authorizationViewModel.RoleIds)
                {
                    autRole = new AuthorizationRole();
                    autRole.Role = _roleUnitOfWork.RoleRepository.GetById(autRoleId);
                    autRole.Authorization = newAuthorization;
                    newAuthorization.AuthorizationRoles.Add(autRole);
                }
            }

            _roleUnitOfWork.AuthorizationRepository.Add(newAuthorization);
            _roleUnitOfWork.Complete();
            return Ok();
        }

        [HttpDelete]
        public async void Delete(int key)
        {
            var autRoleConnections =
                _roleUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(key);
            foreach (var autRole in autRoleConnections)
            {
                _roleUnitOfWork.AuthorizationRoleRepository.Remove(autRole);
            }
            _roleUnitOfWork.AuthorizationRepository.Remove(key);
            _roleUnitOfWork.Complete();
        }

        [HttpGet]
        public async Task<object> GetAuthorizationRoles(int Id, DataSourceLoadOptions loadOptions)
        {

            var roles = await _roleUnitOfWork.RoleRepository.GetAll();

            //IList<Role> rolesBag = new List<Role>();

            //foreach (var role in roles)
            //{
            //    if (role.SubdomainId == null || role.SubdomainId == Id)
            //        rolesBag.Add(role);

            //}

            return DataSourceLoader.Load(roles, loadOptions);
        }
    }
}
