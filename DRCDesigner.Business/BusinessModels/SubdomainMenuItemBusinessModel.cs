using System;
using System.Collections.Generic;
using System.Text;

namespace DRCDesigner.Business.BusinessModels
{
    public class SubdomainMenuItemBusinessModel
    {
        public int Id { get; set; }
        public string text { get; set; }
        public bool disabled { get; set; }
        public string path { get; set; }
        public int type { get; set; }

        public bool EditLock { get; set; }
        public IEnumerable<SubdomainMenuItemBusinessModel> items { get; set; }
    }
}
