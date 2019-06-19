using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
  public interface IResponsibilityCollaborationRepository: IRepository<ResponsibilityCollaboration>
    {
        IEnumerable<ResponsibilityCollaboration> getDrcCardAllResponsibilityCollaboration(int id);
    }
}
