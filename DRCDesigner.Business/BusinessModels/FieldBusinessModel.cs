using System;
using System.Collections.Generic;
using System.Text;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.BusinessModels
{
   public class FieldBusinessModel
    {
        public int Id { get; set; }
        public string AttributeName { get; set; }
        public FieldType Type { get; set; }
        public int DrcCardId { get; set; }
        public int? CollaborationId { get; set; }
        public DrcCard CollaborationCard { get; set; }
        public DrcCard DrcCard { get; set; }
    }
}
