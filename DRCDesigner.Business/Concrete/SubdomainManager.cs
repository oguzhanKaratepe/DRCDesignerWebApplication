using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class SubdomainManager : ISubdomainService
    {
        private ISubdomainUnitOfWork _subdomainUnitOfWork;
        private ISubdomainVersionService _subdomainVersionService;
        private IMapper _mapper;
  
        public SubdomainManager(ISubdomainUnitOfWork subdomainUnitOfWork, ISubdomainVersionService subdomainVersionService, IMapper mapper)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;
            _subdomainVersionService = subdomainVersionService;
            _mapper = mapper;
        }

        public IEnumerable<Subdomain> GetAll()
        {
            return  _subdomainUnitOfWork.SubdomainRepository.GetAll();
        }

        public void Add(string values)
        {
            var newSubdomain = new Subdomain();
            JsonConvert.PopulateObject(values, newSubdomain);
           
            _subdomainUnitOfWork.SubdomainRepository.Add(newSubdomain);
            _subdomainUnitOfWork.Complete();


            var globalRoles = _subdomainUnitOfWork.RoleRepository.getGlobalRoles();
            if (!globalRoles.Any())
            {
                var role = new Role();
                role.RoleName = "Admin";
                role.IsGlobal = true;
                _subdomainUnitOfWork.RoleRepository.Add(role);
                _subdomainUnitOfWork.Complete();
            }

       

        }
        public void Update(string values, int id)
        {
            var subdomain = _subdomainUnitOfWork.SubdomainRepository.GetById(id);
            JsonConvert.PopulateObject(values, subdomain);
            _subdomainUnitOfWork.SubdomainRepository.Update(subdomain);
            _subdomainUnitOfWork.Complete();

        }
        public async Task<bool> Remove(int subdomainId)
        {
            if (subdomainId > 0)
            {
                var subdomain = _subdomainUnitOfWork.SubdomainRepository.GetById(subdomainId);
                var subdomainVersions =
                    await _subdomainUnitOfWork.SubdomainVersionRepository.GetAllSubdomainVersionsBySubdomainId(
                        subdomainId);

                foreach (var version in subdomainVersions)
                {
                     _subdomainVersionService.Remove(version.Id);
                }

                _subdomainUnitOfWork.SubdomainRepository.Remove(subdomain);
                _subdomainUnitOfWork.Complete();
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<SubdomainVersionBusinessModel>> GetMoveDropDownBoxSubdomains(int subdomainVersionId)
        {

            IList<SubdomainVersionBusinessModel> allVersions = new List<SubdomainVersionBusinessModel>();
            if (subdomainVersionId > 0)
            {
                var currentSubdomain = _subdomainUnitOfWork.SubdomainVersionRepository.GetById(subdomainVersionId);
                var subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAllWithVersions();
                foreach (var subdomain in subdomains)
                {
                    if (subdomain.Id != currentSubdomain.SubdomainId)
                    {
                        foreach (var subdomainVersion in subdomain.SubdomainVersions)
                        {
                            var subdomainversionBModel = _mapper.Map<SubdomainVersionBusinessModel>(subdomainVersion);

                            var subdomainName= _subdomainUnitOfWork.SubdomainRepository.GetSubdomainName(subdomainversionBModel.SubdomainId);
                            var versionNumber = subdomainversionBModel.VersionNumber;

                            subdomainversionBModel.SubdomainName = subdomainName + " > " + versionNumber;
                         
                            if (!subdomainversionBModel.EditLock)
                            {
                                allVersions.Add(subdomainversionBModel);
                            }
                            
                        }
                    }
                }

            }

            return allVersions;
        }

        public async Task<IEnumerable<SubdomainVersion>> GetAllSubdomainVersions(int subdomainId)
        {
           return await _subdomainUnitOfWork.SubdomainVersionRepository.GetAllSubdomainVersionsBySubdomainId(subdomainId);
        }
    }
}