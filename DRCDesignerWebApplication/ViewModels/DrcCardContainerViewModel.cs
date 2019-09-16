using System;
using System.Collections.Generic;

namespace DRCDesignerWebApplication.ViewModels
{
    public class DrcCardContainerViewModel
    {
        public DrcCardContainerViewModel()
        {
            DrcCardViewModes = new List<DrcCardViewModel>();
            DrcCardViewModel=new DrcCardViewModel();
        }
        public  IList<DrcCardViewModel> DrcCardViewModes { get; set; }
        public DrcCardViewModel DrcCardViewModel { get; set; }
      
        public int TotalSubdomainSize { get; set; }
        public string SubdomainName { get; set; }
        public string VersionNumber { get; set; }



    }
}