using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DRCDesigner.Business.BusinessModels;

namespace DRCDesigner.Business.Abstract
{
    public interface IExportService
    {
  
        string[] generateSubdomainVersionReportHtml(int subdomainId);
        byte[] generateSubdomainVersionDocuments(int subdomainId);
        IEnumerable<DexmoVersionBusinessModel> getDexmoVersionOptions();
    }
}
