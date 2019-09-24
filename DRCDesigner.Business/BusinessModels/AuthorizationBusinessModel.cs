using System;
using System.Collections.Generic;
using System.Text;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.BusinessModels
{
    public class AuthorizationBusinessModel
    {
        public AuthorizationBusinessModel()
        {
            RoleIds = new int[0];
            Roles = new List<Role>();
        }
        public int Id { get; set; }
        public int DrcCardId { get; set; }
        [JsonIgnore]
        public virtual DrcCard DrcCard { get; set; }
        public string OperationName { get; set; }
        public int[] RoleIds { get; set; }
        public IList<Role> Roles { get; set; }
    }
}
