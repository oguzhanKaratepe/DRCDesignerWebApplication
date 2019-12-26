using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.Entities.Concrete;


namespace DRCDesigner.Business
{
    public class BusinessAutoMapperProfiles : Profile
    {
        public BusinessAutoMapperProfiles()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<SubdomainVersion, SubdomainVersionBusinessModel>();
            CreateMap<SubdomainVersionBusinessModel, SubdomainVersion>();
            CreateMap<FieldBusinessModel, Field>();
            CreateMap<Field, FieldBusinessModel>();
            CreateMap<ResponsibilityBusinessModel, Responsibility>();
            CreateMap<Responsibility, ResponsibilityBusinessModel>();
            CreateMap<AuthorizationBusinessModel, Authorization>();
            CreateMap<Authorization, AuthorizationBusinessModel>();
            CreateMap<DrcCardBusinessModel, DrcCard>();
            CreateMap<DrcCard, DrcCardBusinessModel>();
            CreateMap<SubdomainVersion, SubdomainVersion>();
            CreateMap<Role, RoleBusinessModel>();
            CreateMap<RoleBusinessModel, Role>();
            CreateMap<Field, Field>();
        }
    }
}
