using DogeFriendsAPI.Dto;

namespace DogeFriendsAPI.Data
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserAsync(int id);
        public Task<List<User>> GetUsersAsync(string username);
        public Task<List<PersonDto>> GetPersonsAsync(string fullname);
        public Task<User> InsertUserAsync(User user);
        public Task<User?> UpdateUserAsync(User user);
        public Task<Boolean> DeleteUserAsync(int id);
        public Task SaveChangesAsync();
    }
}