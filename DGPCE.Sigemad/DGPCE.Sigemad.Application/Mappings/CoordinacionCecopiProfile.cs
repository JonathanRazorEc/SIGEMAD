using AutoMapper;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Mappings;
public class CoordinacionCecopiProfile : Profile
{
    public CoordinacionCecopiProfile()
    {
        // Mapeo de Entity a DTO de lectura
        CreateMap<CoordinacionCecopi, CoordinacionCecopiDto>()
            .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Provincia))
            .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio));

        // Mapeo de DTO de escritura a Entity - INCLUIR CAMPOS UTM
        CreateMap<CreateOrUpdateCoordinacionCecopiDto, CoordinacionCecopi>()
            .ForMember(dest => dest.UTM_X, opt => opt.MapFrom(src => src.UTM_X))
            .ForMember(dest => dest.UTM_Y, opt => opt.MapFrom(src => src.UTM_Y))
            .ForMember(dest => dest.Huso, opt => opt.MapFrom(src => src.Huso))
            .ForMember(dest => dest.GeoPosicion, opt => opt.MapFrom(src => src.GeoPosicion))
            .ForMember(dest => dest.Provincia, opt => opt.Ignore())
            .ForMember(dest => dest.Municipio, opt => opt.Ignore())
            .ForMember(dest => dest.Registro, opt => opt.Ignore());

        // Mapeo de Entity a DTO de escritura - INCLUIR CAMPOS UTM
        CreateMap<CoordinacionCecopi, CreateOrUpdateCoordinacionCecopiDto>()
            .ForMember(dest => dest.UTM_X, opt => opt.MapFrom(src => src.UTM_X))
            .ForMember(dest => dest.UTM_Y, opt => opt.MapFrom(src => src.UTM_Y))
            .ForMember(dest => dest.Huso, opt => opt.MapFrom(src => src.Huso))
            .ForMember(dest => dest.GeoPosicion, opt => opt.MapFrom(src => src.GeoPosicion));
    }
}

