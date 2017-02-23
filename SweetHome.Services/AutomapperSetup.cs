using System;
using AutoMapper;

using SweetHome.Core.DTO;
using SweetHome.Core.Entities.Identity;


namespace SweetHome.Services
{
    public class AutomapperSetup
    {
        public static void Initialize()
        {
            Mapper.CreateMap<RegisterUserDTO, AppUser>()
                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dto => dto.LockoutEndDateUtc, opt => opt.UseValue(DateTime.UtcNow))
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            Mapper.CreateMap<AppUser, AppUser>();
        }
    }
}
