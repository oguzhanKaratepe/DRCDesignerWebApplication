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

        [Display(Name = "Byte")]
        Byte = 1,

        [Display(Name = "Integer")]
        Integer =2,

        [Display(Name = "Double")]
        Double =3,

        [Display(Name = "Decimal")]
        Decimal = 4,

        [Display(Name = "Long")]
        Long = 5,

        [Display(Name = "Bool")]
        Bool = 6,

        [Display(Name = "Enum")]
        Enum = 7,

        [Display(Name = "DateOnly")]
        DateOnly = 8,

        [Display(Name = "Time")]
        Time = 9,

        [Display(Name = "DateTime")]
        DateTime =10,

        [Display(Name = "RelationElement")]
        RelationElement = 11,

        [Display(Name = "ComplexTypeElement")]
        ComplexTypeElement = 12,

        [Display(Name = "DetailElement")]
        DetailElement = 13,

        [Display(Name = "Measurement")]
        Measurement = 14,

        [Display(Name = "DynamicField")]
        DynamicField =15

        


    }
    
}
