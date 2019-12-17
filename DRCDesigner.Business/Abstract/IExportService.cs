using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DRCDesigner.Business.Abstract
{
    public interface IExportService
    {
  
        string generateSubdomainVersionReportUrl(int subdomainId);
        void generateSubdomainVersionDocuments(int subdomainId);
    }
}
