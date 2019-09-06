using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.Concrete
{
    public class SubdomainManager : ISubdomainService
    {
        private ISubdomainUnitOfWork _subdomainUnitOfWork;

        public SubdomainManager(ISubdomainUnitOfWork subdomainUnitOfWork)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;
        }

        public async Task<IEnumerable<Subdomain>> GetAll()
        {
            return await _subdomainUnitOfWork.SubdomainRepository.GetAll();
        }

 
        public void Add(string values)
        {
            var newSubdomain = new Subdomain();
            JsonConvert.PopulateObject(values, newSubdomain);
            _subdomainUnitOfWork.SubdomainRepository.Add(newSubdomain);
            _subdomainUnitOfWork.Complete();
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
                var cards = await _subdomainUnitOfWork.DrcCardRepository.getAllCardsBySubdomain(subdomainId);

                foreach (var drcCard in cards)
                {
                    _subdomainUnitOfWork.DrcCardRepository.Remove(drcCard);
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

        public async Task<IEnumerable<Subdomain>> GetMoveDropDownBoxSubdomains(int subdomainId)
        {

            var subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAll();
           var subdomainCollection = new List<Subdomain>();

            foreach (var subdomain in subdomains)
            {
                if (subdomain.Id != subdomainId)
                {
                    subdomainCollection.Add(subdomain);
                }
                else
                {
                    //do not add current subdomain
                }
            }

            return subdomainCollection;
        }
    }
}