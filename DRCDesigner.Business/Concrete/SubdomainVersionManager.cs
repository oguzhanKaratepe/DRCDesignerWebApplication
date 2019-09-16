using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class SubdomainVersionManager : ISubdomainVersionService
    {
        private ISubdomainUnitOfWork _subdomainUnitOfWork;

        private IMapper _mapper;
        public SubdomainVersionManager(ISubdomainUnitOfWork subdomainUnitOfWork,IMapper mapper)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;
            _mapper = mapper;
        }

        public void Add(string values)
        {
            var newSubdomainVersionBModel = new SubdomainVersionBusinessModel();
            JsonConvert.PopulateObject(values, newSubdomainVersionBModel);

            var subdomainVersionModel = _mapper.Map<SubdomainVersion>(newSubdomainVersionBModel);

            if (newSubdomainVersionBModel.ReferencedVersionIds != null)
            {
                foreach (var referenceId in newSubdomainVersionBModel.ReferencedVersionIds)
                {
                    var referencedVersion = _subdomainUnitOfWork.SubdomainVersionRepository.GetById(referenceId);
                    subdomainVersionModel.ReferencedSubdomainVersions.Add(referencedVersion);
                }
            }

            _subdomainUnitOfWork.SubdomainVersionRepository.Add(subdomainVersionModel);
            _subdomainUnitOfWork.Complete();
        }

        public async Task<IEnumerable<SubdomainVersionBusinessModel>> GetAllSubdomainVersions(int subdomainId)
        {
            IList<SubdomainVersionBusinessModel> versionsBusinessModels=new List<SubdomainVersionBusinessModel>();
            
            var versions =await _subdomainUnitOfWork.SubdomainVersionRepository.GetAllSubdomainVersionsBySubdomainId(subdomainId);
            
            foreach (var version in versions)
            {
                var versiyonBusinessModel = _mapper.Map<SubdomainVersionBusinessModel>(version);
                int i = 0;
                int[] tempIds = new int[version.ReferencedSubdomainVersions.Count];
                foreach (var referencedVersion in version.ReferencedSubdomainVersions)
                {
                    tempIds[i] = referencedVersion.Id;
                        i++;
                    
                }

                versiyonBusinessModel.ReferencedVersionIds = tempIds;
                versionsBusinessModels.Add(versiyonBusinessModel);
            }

            return versionsBusinessModels;
        }

        public async Task<IList<SubdomainVersionBusinessModel>> GetReferenceOptions(int subdomainId)
        {
            IList<SubdomainVersionBusinessModel> allVersions = new List<SubdomainVersionBusinessModel>();
            if (subdomainId > 0)
            {
                var subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAllWithVersions();
                foreach (var subdomain in subdomains)
                {
                    foreach (var subdomainVersion in subdomain.SubdomainVersions)
                    {
                        var subdomainversionBModel = _mapper.Map<SubdomainVersionBusinessModel>(subdomainVersion);
                        subdomainversionBModel.SubdomainName =
                            _subdomainUnitOfWork.SubdomainRepository.GetSubdomainName(
                                subdomainversionBModel.SubdomainId);
                        allVersions.Add(subdomainversionBModel);
                        
                    }
                    
                }
               
            }

            return allVersions;
        }

        public async Task<bool> Remove(int subdomainVersionId)
        {
            if (subdomainVersionId> 0)
            {
                var subdomainVersion = _subdomainUnitOfWork.SubdomainVersionRepository.GetById(subdomainVersionId);
                var cards =
                    await _subdomainUnitOfWork.DrcCardRepository.getAllCardsBySubdomainVersion(subdomainVersionId);

                foreach (var drcCard in cards)
                {
                    _subdomainUnitOfWork.DrcCardRepository.Remove(drcCard);
                }

                _subdomainUnitOfWork.SubdomainVersionRepository.Remove(subdomainVersion);
                _subdomainUnitOfWork.Complete();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void Update(string values, int id)
        {

            var subdomainVersion = await _subdomainUnitOfWork.SubdomainVersionRepository.GetVersionWithReferencesById(id);
           
           
            _subdomainUnitOfWork.SubdomainVersionRepository.Update(subdomainVersion);
            _subdomainUnitOfWork.Complete();
            
            
        }
    }
}
