using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesigner.Entities.Concrete
{
    public enum FieldType
    {
        [Display(Name = "String")]
        String =0,

        [Display(Name = "Integer")]
        Integer =1,

        [Display(Name = "Double")]
        Double =2,

        [Display(Name = "Bool")]
        Bool = 3,

        [Display(Name = "Enum")]
        Enum = 4,

        [Display(Name = "Decimal")]
        Decimal =5,

        [Display(Name = "DateTime")]
        DateTime =6,

        [Display(Name = "RelationElement")]
        RelationElement = 7,

        [Display(Name = "ComplexTypeElement")]
        ComplexTypeElement = 8,

        [Display(Name = "DetailElement")]
        DetailElement = 9


    }
}
