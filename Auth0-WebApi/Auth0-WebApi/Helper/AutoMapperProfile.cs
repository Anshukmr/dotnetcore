using AutoMapper;
using Auth0.WebApi.Models;
using Auth0.WebApi.Models.Request;
using Auth0.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DocumentInput, Document>();
            CreateMap<Document, DocumentInput>();

            CreateMap<Document, DocumentOutput>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dto => dto.Path, opt => opt.MapFrom(src => src.URI))
                 .ForMember(dto => dto.DocType, opt => opt.MapFrom(src =>src.DocType));


            CreateMap<DocumentOutput, Document>();

            CreateMap<UserInput, User>();
        }
    }
}
