namespace DogeFriendsAPI.Data
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<User?> GetUser(int id);
        public Task<User> InsertUser(User user);
        public Task<User?> UpdateUser(User user);
        public Task<Boolean> DeleteUser(int id);
        public Task SaveChanges();
    }
}