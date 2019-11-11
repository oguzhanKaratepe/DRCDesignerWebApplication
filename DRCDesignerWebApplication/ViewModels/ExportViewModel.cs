using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.ViewModels
{
    public class ExportViewModel
    {
        [Required]
        [DisplayName("Export Version:")]
        public int VersionId { get; set; }

    }
}
