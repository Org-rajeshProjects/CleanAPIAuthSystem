using AutoMapper;
using CleanAPIAuthSystem.Application.DTOs.Users;
using CleanAPIAuthSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Mappings
{
    /// <summary>
    /// AutoMapper Profile - Defines Entity to DTO mappings
    /// Theory: AutoMapper uses reflection to copy properties
    /// If property names match, mapping is automatic
    /// For custom mapping, use ForMember()
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User -> UserDto mapping
            // Theory: CreateMap<Source, Destination>()
            // AutoMapper copies matching properties automatically
            CreateMap<User, UserDto>()
                // Custom mapping for LinkedProviders
                // Theory: ForMember specifies how to map a property
                // MapFrom uses a lambda to compute the value
                .ForMember(
                    dest => dest.LinkedProviders,
                    opt => opt.MapFrom(src => src.SocialLogins.Select(sl => sl.Provider).ToList())
                );

            // Add more mappings as needed
            // Example: CreateMap<Order, OrderDto>()
        }
    }
}
