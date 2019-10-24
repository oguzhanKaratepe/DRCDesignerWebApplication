using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DRCDesigner.Entities.Concrete
{
   
    public enum ESecurityCriticalOptions
    {


        /// <summary>
        /// Both read and write operations are critical for document or document members
        /// </summary>
        [Display(Name = "ReadandWrite")]
        ReadandWrite = 0,

        /// <summary>
        /// Write is a critical security operation for document or document members
        /// </summary>
      [Display(Name = "Write")]
        Write = 1,

        /// <summary>
        /// Read is a critical security operation for document or document members
        /// </summary>
        [Display(Name = "Read")]
        Read = 2,

     
            /// <summary>
            /// No security critical operation for document or document members
            /// </summary>
            [Display(Name = "No")]
        No = 3
    }

}
