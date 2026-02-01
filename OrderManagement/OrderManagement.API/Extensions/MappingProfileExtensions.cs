using AutoMapper;
using OrderManagement.API.DTOs;
using OrderManagement.Entities.Entities;

namespace OrderManagement.API.Extensions
{
    public class MappingProfileExtensions : Profile
    {
        public MappingProfileExtensions()
        {
            CreateMap<Producto, ProductoDTo>().ReverseMap();
            CreateMap<Cliente, ClienteDto>().ReverseMap();

            CreateMap<Orden, OrdenDto>()
                .ForMember(dest => dest.Detalles,
                           opt => opt.MapFrom(src => src.DetalleOrden))
                .ReverseMap();

            CreateMap<DetalleOrden, DetalleOrdenDto>().ReverseMap();
        }
    }
}