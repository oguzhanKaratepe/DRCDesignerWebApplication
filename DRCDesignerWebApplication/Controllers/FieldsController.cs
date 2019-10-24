using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
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
        private IFieldService _fieldService;
        public FieldsController(IFieldService fieldService, IMapper mapper)
        {
            _mapper = mapper;
            _fieldService = fieldService;
        }

        [HttpGet]
        public async Task<object> GetCollaborations(int Id,int cardId, DataSourceLoadOptions loadOptions)
        {
            var cards = await _fieldService.GetCollaborations(Id, cardId);
            return DataSourceLoader.Load(cards, loadOptions);
        }

        [HttpGet]
        public async Task<object> Get(int cardId, DataSourceLoadOptions loadOptions)
        {
            var fieldBusinessModels =await _fieldService.GetCardFields(cardId);
            var fieldViewModels= _mapper.Map<IList<FieldBusinessModel>, IList<FieldViewModel>>(fieldBusinessModels);

            return DataSourceLoader.Load(fieldViewModels, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            if (ModelState.IsValid) { 
                _fieldService.Add(values);
            }
            else { 
                return BadRequest("I will add error to here");
            }
           
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            if (ModelState.IsValid)
            {
                _fieldService.Update(key,values);
            }
            else
            {
                return BadRequest("I will add error to here");
            }
           
            return Ok();
        }
        [HttpDelete]
        public void Delete(int key)
        {
            _fieldService.Delete(key);
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

        public static List<object> GetSecurityCriticalOptions()
        {
            var items = new List<object>();
            foreach (var item in Enum.GetValues(typeof(ESecurityCriticalOptions)))
            {
                var fieldInfo = item.GetType().GetField(item.ToString());
                var descriptionAttributes = fieldInfo.GetCustomAttributes(
                    typeof(DisplayAttribute), false) as DisplayAttribute[];
                items.Add(new { Id = item, Name = descriptionAttributes[0].Name });
            }
            return items;
        }
        public static List<object> GetMeasurementTypes()
        {
            var items = new List<object>();
            foreach (var item in Enum.GetValues(typeof(EMeasurementTypes)))
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
