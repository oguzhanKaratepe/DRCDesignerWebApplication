using System.Collections.Generic;
using DRCDesigner.Core.Entities;
using Newtonsoft.Json;

namespace DRCDesigner.Entities.Concrete
{
    public class Authorization:IEntity
    {
        public Authorization()
        {
            AuthorizationRoles = new List<AuthorizationRole>();
        }
        public int Id { get; set; }
        public int DrcCardId { get; set; }
        [JsonIgnore]
        public virtual DrcCard DrcCard { get; set; }
        public string OperationName { get; set; }
        public virtual IList<AuthorizationRole> AuthorizationRoles { get; set; }


    }
}