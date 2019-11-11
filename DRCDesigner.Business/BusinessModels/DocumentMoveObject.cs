using System;
using System.Collections.Generic;
using System.Text;

namespace DRCDesigner.Business.BusinessModels
{
    public class DocumentMoveObject
    {  public int DrcCardIdToMove { get; set; }
        public int TargetSubdomainVersionId { get; set; }
        public string NewDocumentName { get; set; }
        public MoveResultType MoveResultType { get; set; }
        public string MoveResultDefinition { get; set; }
    }

    public enum MoveResultType {

        DocumentHasConnections=0,
        TargetVersionReferenceProblem=1,
        Fail=2,
        Success=3
    }
}
