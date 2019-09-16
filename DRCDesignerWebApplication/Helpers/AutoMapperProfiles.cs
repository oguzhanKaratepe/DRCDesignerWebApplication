using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.Entities.Concrete;
using DRCDesignerWebApplication.ViewModels;

namespace DRCDesignerWebApplication.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Responsibility, ResponsibilityViewModel>();
            CreateMap<ResponsibilityViewModel, Responsibility>();
            CreateMap<DrcCard,DrcCardViewModel>();
            CreateMap<DrcCardViewModel,DrcCard>();
            CreateMap<Authorization, AuthorizationViewModel>();
            CreateMap<AuthorizationViewModel, Authorization>();
            CreateMap<Field, FieldViewModel>();
            CreateMap<FieldViewModel, Field>();
            CreateMap<ShadowCardSelectBoxBusinessModel, ShadowCardSelectBoxViewModel>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.DrcCardName, opt => opt.MapFrom(src => src.DrcCardName))
                .ForMember(dto => dto.SubdomainId, opt => opt.MapFrom(src => src.SubdomainId))
                .ForMember(dto => dto.SubdomainName, opt => opt.MapFrom(src => src.SubdomainName));

            CreateMap<FieldBusinessModel, FieldViewModel>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.AttributeName, opt => opt.MapFrom(src => src.AttributeName))
                .ForMember(dto => dto.CollaborationCard, opt => opt.MapFrom(src => src.CollaborationCard))
                .ForMember(dto => dto.CollaborationId, opt => opt.MapFrom(src => src.CollaborationId))
                .ForMember(dto => dto.DrcCardId, opt => opt.MapFrom(src => src.DrcCardId))
                .ForMember(dto => dto.DrcCard, opt => opt.MapFrom(src => src.DrcCard))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<SubdomainVersionViewModel, SubdomainVersionBusinessModel>();
            CreateMap<SubdomainVersionBusinessModel, SubdomainVersionViewModel>();


        }
    }
}
