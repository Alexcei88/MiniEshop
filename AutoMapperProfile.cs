using AutoMapper;
using MiniEshop.Domain;
using MiniEshop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop
{
    public class AutoMapperProfile
        : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GoodDTO, Good>()
                     .ForMember(g => g.Id, member => member.MapFrom(s => s.Id ?? Guid.Empty))
                     .ForMember(g => g.Category, member => member.Ignore())
                     .ForMember(g => g.ImageUrlId, member => member.MapFrom(g => g.FileLink == null ? Guid.Empty : g.FileLink.Id));

            CreateMap<Good, GoodDTO>()
                .ForMember(g => g.FileLink, m => m.MapFrom(g => g.FileLink == null ? new FileLink() : g.FileLink));

            CreateMap<FileLinkDTO, FileLink>().ReverseMap();
        }
    }
}
