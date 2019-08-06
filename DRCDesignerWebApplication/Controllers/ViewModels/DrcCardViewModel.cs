using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesignerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace DRCDesignerWebApplication.Controllers.ViewModels
{
    public class DrcCardViewModel
    {
        public DrcCardViewModel()
        {
            DrsCards =new List<DrcCard>();
            Fields = new List<Field>();
            Responsibilities = new List<Responsibility>();
            Authorizations = new List<Authorization>();
        }
        public  IList<DrcCard> DrsCards { get; set; }
        public DrcCard DrcCard { get; set; }
        public int TotalSubdomainSize { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<Responsibility> Responsibilities { get; set; }
        public virtual ICollection<Authorization> Authorizations { get; set; }

    }
}