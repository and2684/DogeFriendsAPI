using DogeFriendsAPI.Dto;

namespace DogeFriendsAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}