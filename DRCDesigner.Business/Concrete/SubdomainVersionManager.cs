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
        private IDrcUnitOfWork _drcUnitOfWork;
        private IMapper _mapper;

        public SubdomainVersionManager(ISubdomainUnitOfWork subdomainUnitOfWork, IDrcUnitOfWork drcUnitOfWork,
            IMapper mapper)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;
            _drcUnitOfWork = drcUnitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateNewVersionWithSourceVersion(SubdomainVersion newVersion)
        {
            if (newVersion.SourceVersionId != null)
            {
                var sourveVersionId = (int)newVersion.SourceVersionId;

                var sourceVersion =
                    await _subdomainUnitOfWork.SubdomainVersionRepository.GetVersionWithReferencesById(sourveVersionId);

                foreach (var reference in sourceVersion.ReferencedSubdomainVersions)
                {
                    newVersion.ReferencedSubdomainVersions.Add(reference);
                }

                _subdomainUnitOfWork.SubdomainVersionRepository.Update(newVersion);

                //var sourceVersionDrcCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomainVersion(sourveVersionId);
                //foreach (var drcCard in sourceVersionDrcCards)
                //{
                //    DrcCard newDrcCard = new DrcCard();
                //    newDrcCard = drcCard;

                //    newDrcCard.SubdomainVersion = newVersion;
                //}
            }
            else
            {

            }

            _subdomainUnitOfWork.Complete();
            return true;
        }

        public async Task<bool> Add(string values)
        {
            var newSubdomainVersionBModel = new SubdomainVersionBusinessModel();
            JsonConvert.PopulateObject(values, newSubdomainVersionBModel);

            var subdomainVersionModel = _mapper.Map<SubdomainVersion>(newSubdomainVersionBModel);

            if (subdomainVersionModel.SourceVersionId != null)
            {
                var sourceId = (int)subdomainVersionModel.SourceVersionId;
                var sourceSubdomainVersion =
                    await _subdomainUnitOfWork.SubdomainVersionRepository.GetVersionWithReferencesById(sourceId);
                if (sourceSubdomainVersion.EditLock)
                {
                    return false;
                }
                else
                {
                    foreach (var sourceReference in sourceSubdomainVersion.ReferencedSubdomainVersions)
                    {
                        var newReference = new SubdomainVersionReference();
                        newReference.SubdomainVersion = subdomainVersionModel;
                        newReference.ReferencedVersionId = sourceReference.ReferencedVersionId;
                        subdomainVersionModel.ReferencedSubdomainVersions.Add(newReference);
                    }
                }

            }

            if (newSubdomainVersionBModel.ReferencedVersionIds != null)
            {
                foreach (var referenceId in newSubdomainVersionBModel.ReferencedVersionIds)
                {
                    var newReference = new SubdomainVersionReference();
                    newReference.SubdomainVersion = subdomainVersionModel;
                    newReference.ReferencedVersionId = referenceId;
                    subdomainVersionModel.ReferencedSubdomainVersions.Add(newReference);
                }
            }

            _subdomainUnitOfWork.SubdomainVersionRepository.Add(subdomainVersionModel);
            _subdomainUnitOfWork.Complete();

            if (subdomainVersionModel.SourceVersionId != null)
            {
                CloneSourceVersionToNewVersion(subdomainVersionModel);
            }

            return true;
        }

        public async void CloneSourceVersionToNewVersion(SubdomainVersion subdomainVersion)
        {
            var sourceId = (int)subdomainVersion.SourceVersionId;
            var sourceVersionCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomainVersion(sourceId);

            IList<SourceNewDrcCardMap> sourceNewDrcCardMaps = new List<SourceNewDrcCardMap>();
            foreach (var sourceVersionCard in sourceVersionCards)
            {
                SourceNewDrcCardMap sourceNewDrcCardMap = new SourceNewDrcCardMap();
                sourceNewDrcCardMap.SourceCardId = sourceVersionCard.Id;

                DrcCard newDrcCard = new DrcCard();
                newDrcCard = sourceVersionCard;
                newDrcCard.Id = 0;
                newDrcCard.SubdomainVersionId = 0;
                newDrcCard.SubdomainVersion = subdomainVersion;
                _drcUnitOfWork.DrcCardRepository.Add(newDrcCard);

                sourceNewDrcCardMap.NewCardId = newDrcCard.Id;
                sourceNewDrcCardMaps.Add(sourceNewDrcCardMap);
            }

            foreach (var sourceNewDrcCardMap in sourceNewDrcCardMaps)
            {
                var newDrcCard = _drcUnitOfWork.DrcCardRepository.GetById(sourceNewDrcCardMap.NewCardId);
                var sourceCardResponsibilities =
                    _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByDrcCardId(
                        sourceNewDrcCardMap.SourceCardId);

                foreach (var sourceDrcCardResponsibility in sourceCardResponsibilities)
                {
                    Responsibility newResponsibility = new Responsibility();
                    newResponsibility =
                        await _drcUnitOfWork.ResponsibilityRepository.GetByIdWithoutTracking(sourceDrcCardResponsibility
                            .ResponsibilityId);
                    var sourceResponsibilityCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(newResponsibility.Id);
                    newResponsibility.Id = 0;
                    _drcUnitOfWork.ResponsibilityRepository.Add(newResponsibility);

                    DrcCardResponsibility newDrcCardResponsibility = new DrcCardResponsibility();
                    newDrcCardResponsibility.DrcCard = newDrcCard;
                    newDrcCardResponsibility.Responsibility = newResponsibility;
                    newDrcCardResponsibility.IsRelationCollaboration = false;
                    _drcUnitOfWork.DrcCardResponsibilityRepository.Add(newDrcCardResponsibility);

                    foreach (var sourceResponsibilityCollaboration in sourceResponsibilityCollaborations)
                    {
                        int newDrcCardId = sourceNewDrcCardMaps
                            .Where(c => c.SourceCardId == sourceResponsibilityCollaboration.DrcCardId)
                            .Select(c => c.NewCardId).Single();
                        DrcCardResponsibility newResponsibilityCollaboration = new DrcCardResponsibility();
                        newResponsibilityCollaboration.DrcCardId = newDrcCardId;
                        newResponsibilityCollaboration.Responsibility = newResponsibility;
                        newResponsibilityCollaboration.IsRelationCollaboration =
                            sourceResponsibilityCollaboration.IsRelationCollaboration;
                        _drcUnitOfWork.DrcCardResponsibilityRepository.Add(newResponsibilityCollaboration);
                    }


                }

                var sourceCardFields =_drcUnitOfWork.DrcCardFieldRepository
                    .GetDrcCardFieldsByDrcCardId(sourceNewDrcCardMap.SourceCardId);

                foreach (var sourceCardField in sourceCardFields)
                {
                    Field newField = new Field();
                    newField = await _drcUnitOfWork.FieldRepository.GetByIdWithoutTracking(sourceCardField.FieldId);
                    var sourceFieldCollaboration =
                        _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(newField.Id);
                    newField.Id = 0;
                    _drcUnitOfWork.FieldRepository.Add(newField);

                    DrcCardField newDrcCardField = new DrcCardField();
                    newDrcCardField.DrcCard = newDrcCard;
                    newDrcCardField.Field = newField;
                    newDrcCardField.IsRelationCollaboration = false;
                    _drcUnitOfWork.DrcCardFieldRepository.Add(newDrcCardField);

                    if (sourceFieldCollaboration!=null)
                    {
                        int newCollaborationDrcCardId = sourceNewDrcCardMaps
                        .Where(c => c.SourceCardId == sourceFieldCollaboration.DrcCardId)
                        .Select(c => c.NewCardId).Single();

                    DrcCardField newDrcCardFieldCollaboration = new DrcCardField();
                    newDrcCardFieldCollaboration.DrcCardId = newCollaborationDrcCardId;
                    newDrcCardFieldCollaboration.Field = newField;
                    newDrcCardFieldCollaboration.IsRelationCollaboration = true;
                    _drcUnitOfWork.DrcCardFieldRepository.Add(newDrcCardFieldCollaboration);
                    }
                }

                var sourceCardAuthorizations = await _drcUnitOfWork.AuthorizationRepository.GetAuthorizationsByDrcCardId(sourceNewDrcCardMap.SourceCardId);

                foreach (var sourceCardAuthorization in sourceCardAuthorizations)
                {
                    Authorization newAuthorization=new Authorization();
                    newAuthorization = await _drcUnitOfWork.AuthorizationRepository.GetByIdWithoutTracking(sourceCardAuthorization.Id);
                    var oldAuthorizationRoles=_drcUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(newAuthorization.Id);
                    newAuthorization.Id = 0;
                    newAuthorization.DrcCardId = 0;
                    newAuthorization.DrcCard = newDrcCard;
                    _drcUnitOfWork.AuthorizationRepository.Add(newAuthorization);

                    foreach (var oldAuthorizationRole in oldAuthorizationRoles)
                    {
                        AuthorizationRole authorizationRole=new AuthorizationRole();
                        authorizationRole.AuthorizationId = newAuthorization.Id;
                        authorizationRole.RoleId = oldAuthorizationRole.RoleId;
                        _drcUnitOfWork.AuthorizationRoleRepository.Add(authorizationRole);
                    }
                    
                }
                _drcUnitOfWork.Complete();
            }
        }


        public async Task<IEnumerable<SubdomainVersionBusinessModel>> GetAllSubdomainVersionSourceOptions(int id, int subdomainId)
        {
            IList<SubdomainVersionBusinessModel> versionsBusinessModels = new List<SubdomainVersionBusinessModel>();
            var versions = await _subdomainUnitOfWork.SubdomainVersionRepository.GetAllSubdomainVersionsBySubdomainId(subdomainId);

            foreach (var version in versions)
            {
                var versiyonBusinessModel = _mapper.Map<SubdomainVersionBusinessModel>(version);
                if (versiyonBusinessModel.Id != id)
                {
                    versionsBusinessModels.Add(versiyonBusinessModel);
                }
            }

            return versionsBusinessModels;
        }

        public async Task<IEnumerable<SubdomainVersionBusinessModel>> GetAllSubdomainVersions(int subdomainId)
        {
            IList<SubdomainVersionBusinessModel> versionsBusinessModels = new List<SubdomainVersionBusinessModel>();

            var versions = await _subdomainUnitOfWork.SubdomainVersionRepository.GetAllSubdomainVersionsBySubdomainId(subdomainId);


            foreach (var version in versions)
            {
                var versionWithRefs = await _subdomainUnitOfWork.SubdomainVersionRepository.GetVersionWithReferencesById(version.Id);

                var versiyonBusinessModel = _mapper.Map<SubdomainVersionBusinessModel>(versionWithRefs);
                int i = 0;
                int[] tempIds = new int[version.ReferencedSubdomainVersions.Count];
                foreach (var referencedVersion in version.ReferencedSubdomainVersions)
                {
                    tempIds[i] = referencedVersion.ReferencedVersionId;
                    i++;

                }

                if (version.SourceVersionId != null)
                {
                    foreach (var subdomainVersion in versions)
                    {
                        if (version.SourceVersionId == subdomainVersion.Id)
                        {
                            versiyonBusinessModel.SourceVersionName = subdomainVersion.VersionNumber;
                        }
                    }
                }

                versiyonBusinessModel.ReferencedVersionIds = tempIds;
                versionsBusinessModels.Add(versiyonBusinessModel);
            }

            _subdomainUnitOfWork.Complete();
            return versionsBusinessModels;
        }
        public async Task<IEnumerable<SubdomainVersionBusinessModel>> GetAllVersions()
        {
            IList<SubdomainVersionBusinessModel> versionsBusinessModels = new List<SubdomainVersionBusinessModel>();

            var versions = await _subdomainUnitOfWork.SubdomainVersionRepository.GetAll();

            foreach (var version in versions)
            {
                var versionBusinessModel = _mapper.Map<SubdomainVersionBusinessModel>(version);
                versionBusinessModel.SubdomainName =
                    _subdomainUnitOfWork.SubdomainRepository.GetSubdomainName(versionBusinessModel.SubdomainId);
                versionsBusinessModels.Add(versionBusinessModel);
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
                    if (subdomain.Id != subdomainId)
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

            }

            return allVersions;
        }

        public async Task<bool> Remove(int subdomainVersionId)
        {
            if (subdomainVersionId > 0)
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
            var subdomainVersion = _subdomainUnitOfWork.SubdomainVersionRepository.GetById(id);
            var subdomainReferences = await _subdomainUnitOfWork.SubdomainVersionReferenceRepository.getAllVersionReferences(id);

            JsonConvert.PopulateObject(values, subdomainVersion);


            SubdomainVersionBusinessModel subdomainVersionBusinessModel = new SubdomainVersionBusinessModel();
            subdomainVersionBusinessModel.ReferencedVersionIds = new int[subdomainReferences.Count];
            int i = 0;
            foreach (var reference in subdomainReferences)
            {
                subdomainVersionBusinessModel.ReferencedVersionIds[i] = reference.ReferencedVersionId;
                _subdomainUnitOfWork.SubdomainVersionReferenceRepository.Remove(reference);
                i++;
            }

            JsonConvert.PopulateObject(values, subdomainVersionBusinessModel);

            foreach (var latestRefId in subdomainVersionBusinessModel.ReferencedVersionIds)
            {
                var newReference = new SubdomainVersionReference();
                newReference.ReferencedVersionId = latestRefId;
                newReference.SubdomainVersion = subdomainVersion;
                subdomainVersion.ReferencedSubdomainVersions.Add(newReference);
            }
            _subdomainUnitOfWork.Complete();
        }

        public async Task<bool> LookForSourceChange(int id, string values)
        {
            var subdomainVersion = _subdomainUnitOfWork.SubdomainVersionRepository.GetById(id);

            SubdomainVersion updatedversionInstance = _mapper.Map<SubdomainVersion>(subdomainVersion);
            JsonConvert.PopulateObject(values, updatedversionInstance);

            if (subdomainVersion.SourceVersionId != null)
            {
                if (subdomainVersion.SourceVersionId != updatedversionInstance.SourceVersionId)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
