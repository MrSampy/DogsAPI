using AutoMapper;
using Domain.Entities;
using Services.Abstractions.DTOs;

namespace Services.Mappers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<DogDTO, Dog>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Replace("&", " & ")));
            CreateMap<Dog, DogDTO>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Replace(" & ", "&")));
        }
    }
}
