using AutoMapper;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Mappings;
public class CoordinacionPMAProfile : Profile
{
    public CoordinacionPMAProfile()
    {
        // Mapeo de Entity a DTO de lectura
        CreateMap<CoordinacionPMA, CoordinacionPMADto>()
            .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Provincia))
            .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio));

        // Mapeo de DTO de escritura a Entity - INCLUIR CAMPOS UTM
        CreateMap<CreateOrUpdateCoordinacionPmaDto, CoordinacionPMA>()
            .ForMember(dest => dest.UTM_X, opt => opt.MapFrom(src => src.UTM_X))
            .ForMember(dest => dest.UTM_Y, opt => opt.MapFrom(src => src.UTM_Y))
            .ForMember(dest => dest.Huso, opt => opt.MapFrom(src => src.Huso))
            .ForMember(dest => dest.GeoPosicion, opt => opt.MapFrom(src => src.GeoPosicion))
            .ForMember(dest => dest.Provincia, opt => opt.Ignore())
            .ForMember(dest => dest.Municipio, opt => opt.Ignore())
            .ForMember(dest => dest.Registro, opt => opt.Ignore());

        // Mapeo de Entity a DTO de escritura - INCLUIR CAMPOS UTM
        CreateMap<CoordinacionPMA, CreateOrUpdateCoordinacionPmaDto>()
            .ForMember(dest => dest.UTM_X, opt => opt.MapFrom(src => src.UTM_X))
            .ForMember(dest => dest.UTM_Y, opt => opt.MapFrom(src => src.UTM_Y))
            .ForMember(dest => dest.Huso, opt => opt.MapFrom(src => src.Huso))
            .ForMember(dest => dest.GeoPosicion, opt => opt.MapFrom(src => src.GeoPosicion));
    }
}
