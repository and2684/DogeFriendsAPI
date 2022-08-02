using DogeFriendsAPI.Dto;

namespace API.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User, PersonDto>() // Map from User to PersonDto
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim())); // Собираем полное имя (Имя Фамилия)
        }
    }
}