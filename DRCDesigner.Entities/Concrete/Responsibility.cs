using System.Collections.Generic;
using DRCDesigner.Core.Entities;
using Newtonsoft.Json;

namespace DRCDesigner.Entities.Concrete
{
    public class Responsibility : IEntity
    {
        public Responsibility()
        {
            DrcCardResponsibilities=new List<DrcCardResponsibility>();
        }
      
        public int Id { get; set; }
        public int PriorityOrder { get; set; }
        public string Title { get; set; }
        public string ResponsibilityDefinition { get; set; }
        public bool IsMandatory { get; set; }
        public IList<DrcCardResponsibility> DrcCardResponsibilities { get; set; }

    }
}