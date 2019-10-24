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

        public EMeasurementTypes? MeasurementType { get; set; }
        public string DefaultValue { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public bool CreditCard { get; set; }
        public bool Required { get; set; }
        public bool Unique { get; set; }
        public string RegularExpression { get; set; }


    }
}
