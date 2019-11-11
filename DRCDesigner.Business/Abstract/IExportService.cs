using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DRCDesigner.Business.Abstract
{
    public interface IExportService
    {
        void ExportSubdomainVersionAsHtmlFile(int versionId);
    }
}
