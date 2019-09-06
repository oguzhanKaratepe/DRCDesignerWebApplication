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
        [Display(Name = "Decimal")]
        Decimal =3,
        [Display(Name = "DateTime")]
        DateTime =4,
        [Display(Name = "Enum")]
        Enum =5,
        [Display(Name = "ForeignKey")]
        ForeignKey =6,
        [Display(Name = "DocumentComplexTypeElement")]
        DocumentComplexTypeElement =7,
        [Display(Name = "DocumentDetailElement")]
        DocumentDetailElement =8


    }
}
