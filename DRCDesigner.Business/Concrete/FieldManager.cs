using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
   public class FieldManager:IFieldService
   {
       private IDrcUnitOfWork _drcUnitOfWork;
       private IMapper _mapper;

       public FieldManager(IDrcUnitOfWork drcUnitOfWork,IMapper mapper)
       {
           _drcUnitOfWork = drcUnitOfWork;
           _mapper = mapper;

       }
        public void Add(string values)
        {
            var newFieldBusinessModel = new FieldBusinessModel();
            JsonConvert.PopulateObject(values, newFieldBusinessModel);


            Field field = _mapper.Map<Field>(newFieldBusinessModel);
            _drcUnitOfWork.FieldRepository.Add(field);

            DrcCardField newDrcCardFieldCollaboration = new DrcCardField();
            newDrcCardFieldCollaboration.DrcCard = _drcUnitOfWork.DrcCardRepository.GetById(newFieldBusinessModel.DrcCardId);
            newDrcCardFieldCollaboration.Field = field;
            _drcUnitOfWork.DrcCardFieldRepository.Add(newDrcCardFieldCollaboration);

            if (newFieldBusinessModel.CollaborationId != null)
            {
                DrcCardField newFieldCollaboration = new DrcCardField();
                newFieldCollaboration.DrcCardId = (int)newFieldBusinessModel.CollaborationId;
                newFieldCollaboration.FieldId = field.Id;
                newFieldCollaboration.IsRelationCollaboration = true;
                _drcUnitOfWork.DrcCardFieldRepository.Add(newFieldCollaboration);
            }


            _drcUnitOfWork.Complete();

        }

        public void Update(int id, string values)
        {
            var oldField = _drcUnitOfWork.FieldRepository.GetById(id);
            FieldBusinessModel fieldViewModel = _mapper.Map<FieldBusinessModel>(oldField);

            var fieldCollaboration = _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(oldField.Id);
            if (fieldCollaboration != null)
            {
                fieldViewModel.CollaborationId = fieldCollaboration.DrcCardId;
                _drcUnitOfWork.DrcCardFieldRepository.Remove(fieldCollaboration);
            }

            JsonConvert.PopulateObject(values, fieldViewModel);
            _drcUnitOfWork.FieldRepository.Remove(oldField);
            var updatedField = _mapper.Map<Field>(fieldViewModel);
            if (fieldViewModel.CollaborationId != null)
            {
                var collaborationId = (int)fieldViewModel.CollaborationId;
                var newfieldCollaboration = new DrcCardField();
                newfieldCollaboration.FieldId = updatedField.Id;
                newfieldCollaboration.DrcCardId = collaborationId;
                newfieldCollaboration.IsRelationCollaboration = true;
                _drcUnitOfWork.DrcCardFieldRepository.Add(newfieldCollaboration);
            }
            _drcUnitOfWork.FieldRepository.Add(updatedField);
            _drcUnitOfWork.Complete();
        }

        public void Delete(int id)
        {
            var drcFieldCollections = _drcUnitOfWork.DrcCardFieldRepository.GetAllDrcCardFieldsByFieldId(id);
            foreach (var collection in drcFieldCollections)
            {
                _drcUnitOfWork.DrcCardFieldRepository.Remove(collection);
            }
            _drcUnitOfWork.FieldRepository.Remove(id);
            _drcUnitOfWork.Complete();
        }

        
        public async Task<IList<DrcCard>> GetCollaborations(int versionId, int cardId)
        {
            var drcCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomainVersion(versionId);
            IList<DrcCard> cards = new List<DrcCard>();
            foreach (var card in drcCards)
            {
                if (card.Id != cardId)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

        public async Task<IList<FieldBusinessModel>> GetCardFields(int cardId)
        {
            var drcCardFields = _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldsByDrcCardId(cardId);

            var fieldBusinessModels = new List<FieldBusinessModel>();
            foreach (var drcCardField in drcCardFields)
            {
                var field = _drcUnitOfWork.FieldRepository.GetById(drcCardField.FieldId);
                var fieldBusinessModel = _mapper.Map<FieldBusinessModel>(field);

                DrcCardField drcFieldCollaboration = _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(field.Id);
                if (drcFieldCollaboration != null)
                {
                    var collaborationcard = _drcUnitOfWork.DrcCardRepository.GetById(drcFieldCollaboration.DrcCardId);
                    fieldBusinessModel.CollaborationId = collaborationcard.Id;
                }
                fieldBusinessModels.Add(fieldBusinessModel);
            }

            return fieldBusinessModels;
        }
    }
}
