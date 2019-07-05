using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
    public interface IResponsibilityRepository:IRepository<Responsibility>
    {
        ICollection<Responsibility> getDrcCardAllResponsibilities(int id); //id: drcCard id
    }
}
