using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DRCDesigner.Entities.Concrete
{
   
    public enum EDeleteBehaviorOptions
    {
        /// <summary>
        /// marks the document as deleted.
        /// </summary>
        ///
        [Display(Name = "Soft")]
        Soft = 0,
        /// <summary>
        /// deletes the document from store
        /// </summary>
        [Display(Name = "Hard")]
        Hard = 1
    }

}
