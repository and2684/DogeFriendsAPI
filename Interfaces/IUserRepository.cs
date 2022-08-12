using DogeFriendsAPI.Dto;

namespace DogeFriendsAPI.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserShowDto>> GetAllUsersAsync();
        public Task<UserShowDto?> GetUserAsync(int id);
        public Task<List<UserShowDto>> GetUsersAsync(string username);
        public Task<List<PersonDto>> GetPersonsAsync(string fullname);
        public Task<User> InsertUserAsync(User user);
        public Task<User?> UpdateUserAsync(User user);
        public Task<Boolean> DeleteUserAsync(int id);
        public Task<UserDto> RegisterUser(RegisterDto registerDto);
        public Task<UserDto> LoginUser(LoginDto loginDto);
        public Task<Boolean> UserExist(string username);
        public Task<bool> PasswordCorrect(LoginDto loginDto);
        public Task SaveChangesAsync();
    }
}