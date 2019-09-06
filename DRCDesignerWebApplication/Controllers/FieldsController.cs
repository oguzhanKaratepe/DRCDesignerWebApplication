using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using DRCDesignerWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace DRCDesignerWebApplication.Controllers
{
    public class FieldsController : Controller
    {
        private IMapper _mapper;
        private IDrcUnitOfWork _drcUnitOfWork;
        public FieldsController(IDrcUnitOfWork drcUnitOfWork, IMapper mapper)
        {
            _drcUnitOfWork = drcUnitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<object> GetCollaborations(int Id,int cardId, DataSourceLoadOptions loadOptions)
        {

            var drcCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomain(Id);
            IList<DrcCard> cards = new List<DrcCard>();
            foreach (var card in drcCards)
            {
                if (card.Id != cardId)
                {
                    cards.Add(card);
                }
            }

            return DataSourceLoader.Load(cards, loadOptions);
        }

        [HttpGet]
        public async Task<object> Get(int Id,DataSourceLoadOptions loadOptions)
        {
            var drcCardFields = _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldsByDrcCardId(Id);
    
            var fieldViewModels=new List<FieldViewModel>();
            foreach (var drcCardField in drcCardFields)
            {
                var field = _drcUnitOfWork.FieldRepository.GetById(drcCardField.FieldId);
                var fieldViewModel =_mapper.Map<FieldViewModel>(field);

                DrcCardField drcFieldCollaboration = _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(field.Id);
                if (drcFieldCollaboration != null)
                {
                    var collaborationcard= _drcUnitOfWork.DrcCardRepository.GetById(drcFieldCollaboration.DrcCardId);
                    fieldViewModel.CollaborationId= collaborationcard.Id;
                }
                fieldViewModels.Add(fieldViewModel);
            }
            
            return DataSourceLoader.Load(fieldViewModels, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var newFieldViewModel=new FieldViewModel();
            JsonConvert.PopulateObject(values, newFieldViewModel);
            Field field = _mapper.Map<Field>(newFieldViewModel);
            _drcUnitOfWork.FieldRepository.Add(field);
            if (!TryValidateModel(newFieldViewModel))
                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"

               DrcCardField newDrcCardFieldCollaboration=new DrcCardField();
                newDrcCardFieldCollaboration.DrcCard = _drcUnitOfWork.DrcCardRepository.GetById(newFieldViewModel.DrcCardId);
                newDrcCardFieldCollaboration.Field = field;
                _drcUnitOfWork.DrcCardFieldRepository.Add(newDrcCardFieldCollaboration);

                if (newFieldViewModel.CollaborationId != null)
                {
                    DrcCardField newFieldCollaboration = new DrcCardField();
                    newFieldCollaboration.DrcCardId = (int) newFieldViewModel.CollaborationId;
                    newFieldCollaboration.FieldId = field.Id;
                    newFieldCollaboration.IsRelationCollaboration = true;
                    _drcUnitOfWork.DrcCardFieldRepository.Add(newFieldCollaboration);
            }
            

            _drcUnitOfWork.Complete();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var oldField = _drcUnitOfWork.FieldRepository.GetById(key);
            FieldViewModel fieldViewModel = _mapper.Map<FieldViewModel>(oldField);

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
                  var collaborationId = (int) fieldViewModel.CollaborationId;
                  var newfieldCollaboration=new DrcCardField();
                  newfieldCollaboration.FieldId = updatedField.Id;
                  newfieldCollaboration.DrcCardId = collaborationId;
                  newfieldCollaboration.IsRelationCollaboration = true;
                  _drcUnitOfWork.DrcCardFieldRepository.Add(newfieldCollaboration);
              }
              _drcUnitOfWork.FieldRepository.Add(updatedField);
              _drcUnitOfWork.Complete();
            return Ok();
        }
        [HttpDelete]
        public void Delete(int key)
        {
            var drcFieldCollections = _drcUnitOfWork.DrcCardFieldRepository.GetAllDrcCardFieldsByFieldId(key);
            foreach (var collection in drcFieldCollections)
            {
                _drcUnitOfWork.DrcCardFieldRepository.Remove(collection);
            }

            _drcUnitOfWork.FieldRepository.Remove(key);
            _drcUnitOfWork.Complete();
        }

        public static List<object> GetFieldTypeDataSource()
        {
            var items = new List<object>();
            foreach (var item in Enum.GetValues(typeof(FieldType)))
            {
                var fieldInfo = item.GetType().GetField(item.ToString());
                var descriptionAttributes = fieldInfo.GetCustomAttributes(
                    typeof(DisplayAttribute), false) as DisplayAttribute[];
                items.Add(new { Id = item, Name = descriptionAttributes[0].Name });
            }
            return items;
        }
    }
}
