using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Concrete
{
    public class ExportManager : IExportService
    {
        private readonly IDrcCardService _drcCardService;
        private readonly ISubdomainUnitOfWork _subdomainUnitOfWork;

        public ExportManager(IDrcCardService drcCardService,ISubdomainUnitOfWork subdomainUnitOfWork)
        {
            _drcCardService = drcCardService;
            _subdomainUnitOfWork = subdomainUnitOfWork;
        }
        
        public async void ExportSubdomainVersionAsHtmlFile(int versionId)
        {

            var version = _subdomainUnitOfWork.SubdomainVersionRepository.GetById(versionId);
            var SubdomainName= _subdomainUnitOfWork.SubdomainRepository.GetSubdomainName(version.SubdomainId);

            IList<DrcCardBusinessModel> drcCards = await _drcCardService.GetAllDrcCards(versionId);

            string fileName = SubdomainName + "_" + version.VersionNumber + ".html";
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                   
                    w.WriteLine("<html>");
                    w.WriteLine("<head>");
                    w.WriteLine("<link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css\" integrity=\"sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T\" crossorigin=\"anonymous\">");
                    w.WriteLine("</head>");
                    w.WriteLine("<body style=\"background-color: #222;\">");
                    w.WriteLine("<style>    " +
                                ".bg-info {" +
                                "        background: #9c27b0 !important;" +
                                "    }"+
                                
                                ".caption {" +
                                "        font-size: 16px;" +
                                "        padding-bottom: 3px;" +
                                "        padding-left: 10px;" +
                                "    }" +
                                "    .container-fluid .card {" +
                                "        max-width: 500px;" +
                                "    }" +
                                "    .container-fluid {" +
                                "        display: flex;" +
                                "        flex-wrap: wrap;" +
                                "        min-height: 300px;" +
                                "    }" +
                                "        .container-fluid .card .card-footer {" +
                                "            display: flex;" +
                                "            flex-flow: row wrap;" +
                                "        }" +
                                "        .container-fluid .card-deck {" +
                                "            display: flex;" +
                                "            align-items: flex-start;" +
                                "            align-content: flex-start;" +
                                "            flex-wrap: wrap;" +
                                "        }" +
                                "        .container-fluid .card .card-body {" +
                                "            display: flex;" +
                                "            flex-flow: row wrap;" +
                                "            flex-direction: column;" +
                                "        }" +
                                "        .container-fluid .card .card-footer > div {" +
                                "            margin: 5px;" +
                                "        }" +
                                "    .card-header {" +
                                "        height: 60px;" +
                                "    }" +
                                "   " +
                                "    .btn-circle {" +
                                "        width: 50px;" +
                                "        height: 50px;" +
                                "        line-height: 45px;" +
                                "        text-align: center;" +
                                "        padding: 0;" +
                                "        border-radius: 50%;" +
                                "    }" +
                                "    .card.border-secondary.mb-3.mr-2.mt-2:hover {" +
                                "        border-color: red !important;" +
                                "    }" +
                                "    .btn-circle-sm {" +
                                "        width: 40px;" +
                                "        height: 40px;" +
                                "        line-height: 35px;" +
                                "        font-size: 1.5rem;" +
                                "        font-weight: bold;" +
                                "    }" +
                                "    .btn-circle-xsm {" +
                                "        width: 35px;" +
                                "        height: 35px;" +
                                "        line-height: 30px;" +
                                "        font-size: 1.5rem;" +
                                "        font-weight: bold;" +
                                "    }" +
                                "    .container-fluid {" +
                                "        padding-left: 0px;" +
                                "        padding-right: 0px;" +
                                "    }" +
                                "    #shadowcardheader {" +
                                "        background-color: #808080;" +
                                "    }" +
                                "</style>");
                    w.WriteLine(" <div class=\"justify-content-center pt-5\"> <h3 class=\"  text-light border-bottom rounded-bottom mr-5\">&nbsp;" + SubdomainName +" : "+ version.VersionNumber+"&nbsp; </h3></div>");
                    w.WriteLine(" <div class=\"flex-container d-flex flex-wrap ml-3 align-items-baseline \">");
                    CreateDocumentHtml(w, drcCards);
                    w.WriteLine("</div>");
                    w.WriteLine("</body>");
                    w.WriteLine("</html>");
                    w.Close();
                    
                }
            }
        }

        public void CreateDocumentHtml(StreamWriter w, IEnumerable<DrcCardBusinessModel> drcCards)
        {
          
            foreach (var drcCard in drcCards)
            {
                var responsibilities = _drcCardService.getListOfDrcCardResponsibilities(drcCard.Id);
                var authorizations = _drcCardService.getListOfDrcCardAuthorizations(drcCard.Id);
                var fields = _drcCardService.getListOfDrcCardFields(drcCard.Id,drcCard.MainCardId);
                w.WriteLine(" <div class=\"card border-secondary  mb-3 mr-2 mt-2 d-flex\" style=\"min-width: 28rem; max-width: 36rem;\">");
                if (drcCard.MainCardId != null)
                {
                    w.WriteLine("<div id=\"shadowcardheader\" class=\"card-header  text-light border-secondary d-flex flex-column pb-5\">");
                    w.WriteLine("   <div> <h5 class=\"align-self-start\">" + drcCard.DrcCardName + "</h5> <a class=\"align-self-start p-0\">" + drcCard.SourceDrcCardPath + "</a></div>");
                    w.WriteLine("</div>");
                    w.WriteLine("  <div class=\"card-body text-dark bg-light row m-0 p-0\" style=\"min-height: 11rem\">");
                    CreateResponsibilityHtml(w, responsibilities);
                    w.WriteLine("<div class=\"bg-light border-right border-left rounded-left rounded-right col col-3 pr-0 pl-0\">" + "</div>"); //authorization area
                    CreateCollaborationHtml(w, responsibilities, fields);
                    w.WriteLine("</div>");
                    w.WriteLine(" <div id=\"card-footer\" class=\"border-top rounded-top card-footer  bg-transparent d-flex flex-wrap \" style=\"min-height: 4rem\">");
                    CreateFieldHtml(w, fields);
                    w.WriteLine("</div>");
                  
                }
                else
                {
                    w.WriteLine("  <div class=\"card-header  text-light bg-info border-secondary d-flex flex-wrap justify-content-between \">");
                    w.WriteLine("  <div class=\"h5 d-flex\">" + drcCard.DrcCardName + "</div>");
                    w.WriteLine("</div>");
                    w.WriteLine("  <div class=\"card-body text-dark bg-light row m-0 p-0\" style=\"min-height: 11rem\">");
                    CreateResponsibilityHtml(w, responsibilities);
                    CreateAuthorizationHtml(w, authorizations);
                    CreateCollaborationHtml(w, responsibilities, fields);
                    w.WriteLine("</div>");
                    w.WriteLine(" <div id=\"card-footer\" class=\"border-top rounded-top card-footer  bg-transparent d-flex flex-wrap \" style=\"min-height: 4rem\">");
                    CreateFieldHtml(w, fields);
                    w.WriteLine("</div>");
                   
                }

                w.WriteLine("</div>");
            }
           
        }

        private void CreateCollaborationHtml(StreamWriter w, IList<ResponsibilityBusinessModel> responsibilities, IList<FieldBusinessModel> fields)
        {
            w.WriteLine("<div class=\"bg-light col col-4 pl-0 pr-0\">");
            var cardContainer = new List<DrcCard>();
            foreach(var responsibility in responsibilities)
            {
                foreach (var collaboration in responsibility.ResponsibilityCollaborationCards)
                {
                    if (!cardContainer.Contains(collaboration))
                    {
                        cardContainer.Add(collaboration);
                    }
                }

            }
            foreach(var field in fields)
            {
                if (!cardContainer.Contains(field.CollaborationCard) && field.CollaborationCard != null)
                {
                    if (!field.IsShadowField)
                    {
                        cardContainer.Add(field.CollaborationCard);
                    }

                }
            }
            foreach (var card in cardContainer)
            {
                w.WriteLine("<div class=\"p-1 text-dark border-bottom rounded-bottom\">"+card.DrcCardName+"</div>");
            }
           
            w.WriteLine("</div>");
        }

        private  void CreateResponsibilityHtml(StreamWriter w, IList<ResponsibilityBusinessModel> listOfDrcCardResponsibilities)
        {
            w.WriteLine("<div class=\"bd-highlight bg-light col col-5 pr-0 pl-0\">");
            foreach (var responsibility in listOfDrcCardResponsibilities)
            {
                w.WriteLine("<div class=\"border-bottom rounded-bottom m-0 p-0\">");
                w.WriteLine(" <div class=\"text-truncate pl-2 pr-2 pt-1 pb-1 text-dark\">" + responsibility.Title + "<br></div>");
                w.WriteLine("</div>");
            }
            w.WriteLine("</div>");
        }
        private void CreateAuthorizationHtml(StreamWriter w, IList<AuthorizationBusinessModel> listOfDrcCardAuthorizations)
        {

            w.WriteLine("<div class=\"bg-light border-right border-left rounded-left rounded-right col col-3 pr-0 pl-0\">");
            foreach (var authorization in listOfDrcCardAuthorizations)
            {
                w.WriteLine("<div>");
                w.WriteLine(" <div class=\"p-1 text-dark\">" + authorization.OperationName + "()</div>");
                w.WriteLine("</div>");
            }
            w.WriteLine("</div>");
        }
        private void CreateFieldHtml(StreamWriter w, IList<FieldBusinessModel> listOfDrcCardFields)
        {

            foreach(var field in listOfDrcCardFields)
            {
                w.WriteLine("<div class=\" text-dark border-left border-right border-bottom rounded-bottom rounded-right p-1 m-1 \">"+field.AttributeName+ "</div>");
            }
        }
    }
}
