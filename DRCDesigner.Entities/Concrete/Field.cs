using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core.Entities;
using Newtonsoft.Json;

namespace DRCDesigner.Entities.Concrete
{
    public class Field : IEntity
    {
        public int Id { get; set; }
        public string AttributeName { get; set; }
        public FieldType Type { get; set; }
        public virtual ICollection<DrcCardField> DrcCardFields { get; set; }
    }
}
