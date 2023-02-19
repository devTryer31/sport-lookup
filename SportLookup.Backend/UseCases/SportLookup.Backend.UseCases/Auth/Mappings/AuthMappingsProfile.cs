using AutoMapper;
using SportLookup.Backend.Entities.Models.Auth;
using SportLookup.Backend.UseCases.Auth.DTOs;

namespace SportLookup.Backend.UseCases.Auth.Mappings;

public class AuthMappingsProfile : Profile
{
	public AuthMappingsProfile()
	{
		CreateMap<RegisterUserDTO, AppUser>()
			.ForSourceMember(s => s.Password, opt => opt.DoNotValidate())
			.ForMember(t => t.Email, opt => opt.MapFrom(s => s.EMail));
	}
}
