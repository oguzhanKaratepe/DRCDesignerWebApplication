using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Interfaces
{
    public interface IProjectRepository :IRepository<DrcProject>
    {
       IEnumerable<DrcProject> getProjectSubdomains(int id);
       
    }
}
