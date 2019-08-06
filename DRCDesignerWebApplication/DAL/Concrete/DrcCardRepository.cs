using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Concrete
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

            return DrcCardContext.DrcCards.Include(a => a.Fields).Include(a => a.Responsibilities).Include(a => a.Authorizations).Where(s => s.Id == id).Single();

        }


        public void setDrcCardOrder(int id, int order) //drcCard id and Card order
        {
            DrcCard DrcCard = DrcCardContext.DrcCards.Where(s => s.Id == id).Single();
            DrcCard.Order = order;
            DrcCardContext.SaveChanges();
        }

        async Task<ICollection<DrcCard>> IDrcCardRepository.getAllCardsBySubdomain(int subdomainID)
        {
      
            return await DrcCardContext.DrcCards.Where(s => s.SubdomainId == subdomainID).ToListAsync();
        }

        public void Update(DrcCard drcCard)
        {
            var existingCard = DrcCardContext.DrcCards
                .Where(p => p.Id == drcCard.Id)
                .Include(p => p.Responsibilities)
                .Include(p => p.Authorizations)
                .Include(p => p.Fields)
                .SingleOrDefault();

            if (existingCard != null)
            {
                // Update parent
                DrcCardContext.Entry(existingCard).CurrentValues.SetValues(drcCard);


                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                // Delete Responsibility
                foreach (var existingResponsibility in existingCard.Responsibilities)
                {
                    if (!drcCard.Responsibilities.Any(c => c.Id == existingResponsibility.Id))
                        DrcCardContext.Responsibilities.Remove(existingResponsibility);
                }

                // Update and Insert Responsibility
                foreach (var responsibilityModel in drcCard.Responsibilities)
                {
                    var existingResponsibility = existingCard.Responsibilities
                        .Where(c => c.Id == responsibilityModel.Id)
                        .SingleOrDefault();

                    if (existingResponsibility != null)
                        // Update child
                        DrcCardContext.Entry(existingResponsibility).CurrentValues.SetValues(responsibilityModel);
                    else
                    {
                        // Insert child
                        var newResponsibility = new Responsibility()
                        {
                            ResponsibilityDefinition = responsibilityModel.ResponsibilityDefinition,
                            Title = responsibilityModel.Title,
                            DrcCard = existingCard,
                            DrcCardID = existingCard.Id,
                            ShadowCards = responsibilityModel.ShadowCards,
                            IsMandatory = responsibilityModel.IsMandatory

                        };
                        DrcCardContext.Responsibilities.Add(newResponsibility);
                    }
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////

                // Delete Authorization
                foreach (var existingAuthorization in existingCard.Authorizations)
                {
                    if (!drcCard.Authorizations.Any(c => c.Id == existingAuthorization.Id))
                        DrcCardContext.Authorizations.Remove(existingAuthorization);
                }

                foreach (var AuthorizationModel in drcCard.Authorizations)
                {
                    var existingAuthorization = existingCard.Authorizations
                        .Where(c => c.Id == AuthorizationModel.Id)
                        .SingleOrDefault();

                    if (existingAuthorization != null)
                        // Update child
                        DrcCardContext.Entry(existingAuthorization).CurrentValues.SetValues(AuthorizationModel);
                    else
                    {
                        // Insert child
                        var newAuthorization = new Authorization()
                        {
                            DrcCardId = existingCard.Id,
                            DrcCard = existingCard,
                            OperationName = AuthorizationModel.OperationName,
                            RoleFields = AuthorizationModel.RoleFields

                        };
                        DrcCardContext.Authorizations.Add(newAuthorization);
                    }
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                // Delete Authorization
                foreach (var existingField in existingCard.Fields)
                {
                    if (!drcCard.Fields.Any(c => c.Id == existingField.Id))
                        DrcCardContext.Fields.Remove(existingField);
                }

                foreach (var FieldModel in drcCard.Fields)
                {
                    var existingField = existingCard.Fields
                        .Where(c => c.Id == FieldModel.Id)
                        .SingleOrDefault();

                    if (existingField != null)
                        // Update child
                        DrcCardContext.Entry(existingField).CurrentValues.SetValues(FieldModel);
                    else
                    {
                        // Insert child
                        var newField = new Field()
                        {
                            DrcCardId = existingCard.Id,
                            DrcCard = existingCard,
                            AttributeName = FieldModel.AttributeName,
                            type = FieldModel.type

                        };
                        DrcCardContext.Fields.Add(newField);
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                DrcCardContext.SaveChanges();
            }

        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
