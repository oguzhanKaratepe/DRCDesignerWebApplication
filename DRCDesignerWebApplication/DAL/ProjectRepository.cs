using DRCDesignerWebApplication.DAL.Interfaces;
using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL
{
    public class ProjectRepository : Repository<DrcProject>, IProjectRepository

    {
        public IEnumerable<DrcProject> getProjectSubdomains(int id)
        {
            throw new NotImplementedException();
        }
    }
}
