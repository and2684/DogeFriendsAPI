using DogeFriendsAPI.Dto;

namespace API.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User, PersonDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim())); // Собираем полное имя (Имя Фамилия)

            CreateMap<User, UserShowDto>();

            // Не вышло, маппер не видит hmac. К тому же, логика в маппере - это неправильно.
            // using var hmac = new HMACSHA512();
            // CreateMap<RegisterDto, User>()
            //     .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.ToLower()))
            //     .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => hmac.ComputeHash(Encoding.UTF8.GetBytes(src.Password))))
            //     .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => hmac.Key))
            //     .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            //     .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            //     .ForMember(dest => dest.IsCompany, opt => opt.MapFrom(src => src.IsCompany));    
        }
    }
}