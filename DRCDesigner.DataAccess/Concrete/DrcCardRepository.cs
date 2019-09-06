
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Concrete
{
    public class DrcCardRepository : Repository<DrcCard>, IDrcCardRepository
    {
        public DrcCardRepository(DrcCardContext context) : base(context)
        {
        }

        public int getDrcCardOrder(int id) //drcCard id
        {
            DrcCard DrcCard = DrcCardContext.DrcCards.Where(s => s.Id == id).Single();

            return DrcCard.Order;
        }
        public DrcCard getDrcCardWithAllEntities(int id) //drcCard id
        {

            return DrcCardContext.DrcCards.Include(a => a.DrcCardFields).Include(a => a.DrcCardResponsibilities).ThenInclude(a=>a.Responsibility).Include(a => a.Authorizations).ThenInclude(a=>a.AuthorizationRoles).ThenInclude(a=>a.Authorization).Where(s => s.Id == id).Single();

        }


        public void setDrcCardOrder(int id, int order) //drcCard id and Card order
        {
            DrcCard DrcCard = DrcCardContext.DrcCards.Where(s => s.Id == id).Single();
            DrcCard.Order = order;
            DrcCardContext.SaveChanges();
        }

   
        //public void Remove(DrcCard drcCard)
        //{
        //    var existingCard = DrcCardContext.DrcCards
        //        .Where(p => p.Id == drcCard.Id)
        //        .Include(p => p.Responsibilities)
        //        .Include(p => p.ModelAuthorizations)
        //        .Include(p => p.Fields)
        //        .SingleOrDefault();

        //    if (existingCard != null)
        //    {
        //        foreach (var existingResponsibility in existingCard.Responsibilities)
        //        {
        //            if (!drcCard.Responsibilities.Any(c => c.Id == existingResponsibility.Id))
        //                DrcCardContext.Responsibilities.Remove(existingResponsibility);
        //        }

        //        foreach (var existingAuthorization in existingCard.ModelAuthorizations)
        //        {
        //            if (!drcCard.ModelAuthorizations.Any(c => c.Id == existingAuthorization.Id))
        //                DrcCardContext.ModelAuthorizations.Remove(existingAuthorization);
        //        }

        //        foreach (var existingField in existingCard.Fields)
        //        {
        //            if (!drcCard.Fields.Any(c => c.Id == existingField.Id))
        //                DrcCardContext.Fields.Remove(existingField);
        //        }

        //        DrcCardContext.DrcCards.Remove(existingCard);
        //        DrcCardContext.SaveChanges();
        //    }
        //}

        //public void Update(DrcCard drcCard)
        //{
        //    var existingCard = DrcCardContext.DrcCards
        //        .Where(p => p.Id == drcCard.Id)
        //        .Include(p => p.Responsibilities)
        //        .Include(p => p.ModelAuthorizations)
        //        .Include(p => p.Fields)
        //        .SingleOrDefault();

        //    if (existingCard != null)
        //    {
        //        // Update parent
        //        DrcCardContext.Entry(existingCard).CurrentValues.SetValues(drcCard);


        //        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        //        // Delete Responsibility
        //        foreach (var existingResponsibility in existingCard.Responsibilities)
        //        {
        //            if (!drcCard.Responsibilities.Any(c => c.Id == existingResponsibility.Id))
        //                DrcCardContext.Responsibilities.Remove(existingResponsibility);
        //        }

        //        // Update and Insert Responsibility
        //        foreach (var responsibilityModel in drcCard.Responsibilities)
        //        {
        //            var existingResponsibility = existingCard.Responsibilities
        //                .Where(c => c.Id == responsibilityModel.Id)
        //                .SingleOrDefault();

        //            if (existingResponsibility != null)
        //                // Update child
        //                DrcCardContext.Entry(existingResponsibility).CurrentValues.SetValues(responsibilityModel);
        //            else
        //            {
        //                // Insert child
        //                var newResponsibility = new Responsibility()
        //                {
        //                    ResponsibilityDefinition = responsibilityModel.ResponsibilityDefinition,
        //                    Title = responsibilityModel.Title,
        //                    DrcCard = existingCard,
        //                    DrcCardId = existingCard.Id,
        //                    ShadowCards = responsibilityModel.ShadowCards,
        //                    IsMandatory = responsibilityModel.IsMandatory

        //                };
        //                DrcCardContext.Responsibilities.Add(newResponsibility);
        //            }
        //        }

        //        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        //        // Delete Authorization
        //        foreach (var existingAuthorization in existingCard.ModelAuthorizations)
        //        {
        //            if (!drcCard.ModelAuthorizations.Any(c => c.Id == existingAuthorization.Id))
        //                DrcCardContext.ModelAuthorizations.Remove(existingAuthorization);
        //        }

        //        foreach (var AuthorizationModel in drcCard.ModelAuthorizations)
        //        {
        //            var existingAuthorization = existingCard.ModelAuthorizations
        //                .Where(c => c.Id == AuthorizationModel.Id)
        //                .SingleOrDefault();

        //            if (existingAuthorization != null)
        //                // Update child
        //                DrcCardContext.Entry(existingAuthorization).CurrentValues.SetValues(AuthorizationModel);
        //            else
        //            {
        //                // Insert child
        //                var newAuthorization = new Authorization()
        //                {
        //                    DrcCardId = existingCard.Id,
        //                    DrcCard = existingCard,
        //                    OperationName = AuthorizationModel.OperationName,
        //                    RoleFields = AuthorizationModel.RoleFields

        //                };
        //                DrcCardContext.ModelAuthorizations.Add(newAuthorization);
        //            }
        //        }

        //        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        //        // Delete Authorization
        //        foreach (var existingField in existingCard.Fields)
        //        {
        //            if (!drcCard.Fields.Any(c => c.Id == existingField.Id))
        //                DrcCardContext.Fields.Remove(existingField);
        //        }

        //        foreach (var FieldModel in drcCard.Fields)
        //        {
        //            var existingField = existingCard.Fields
        //                .Where(c => c.Id == FieldModel.Id)
        //                .SingleOrDefault();

        //            if (existingField != null)
        //                // Update child
        //                DrcCardContext.Entry(existingField).CurrentValues.SetValues(FieldModel);
        //            else
        //            {
        //                // Insert child
        //                var newField = new Field()
        //                {
        //                    DrcCardId = existingCard.Id,
        //                    DrcCard = existingCard,
        //                    AttributeName = FieldModel.AttributeName,
        //                    Type = FieldModel.Type

        //                };
        //                DrcCardContext.Fields.Add(newField);
        //            }
        //        }

        //        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //        DrcCardContext.SaveChanges();
        //    }

        //}

        public async Task<IEnumerable<DrcCard>> getAllCardsBySubdomain(int SubdomainID)
        {
            return await DrcCardContext.DrcCards.Where(s => s.SubdomainId == SubdomainID).ToListAsync();
        }

        public IList<DrcCardResponsibility> GetDrcResponsibilities(int id)
        {
            throw new NotImplementedException();
        }


        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
