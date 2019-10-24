using System;
using System.Collections.Generic;
using DRCDesigner.Business.BusinessModels;

namespace DRCDesignerWebApplication.ViewModels
{
    public class DrcCardContainerViewModel
    {
        public DrcCardContainerViewModel()
        {
            DrcCardViewModes = new List<DrcCardViewModel>();
            DrcCardViewModel = new DrcCardViewModel();
            SubdomainMenuItems=new List<SubdomainMenuItemBusinessModel>();
        }
        public IList<DrcCardViewModel> DrcCardViewModes { get; set; }
        public DrcCardViewModel DrcCardViewModel { get; set; }
        public IEnumerable<SubdomainMenuItemBusinessModel> SubdomainMenuItems { get; set; }
        public int TotalSubdomainSize { get; set; }

        public bool IsSubdomainVersionLocked { get; set; }
     



    }
}