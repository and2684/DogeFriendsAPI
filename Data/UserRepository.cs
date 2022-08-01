namespace DogeFriendsAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> InsertUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUser(User user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbUser == null) return null;

            dbUser.Username = user.Username;            
            dbUser.Password = user.Password;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.IsCompany = user.IsCompany;
            dbUser.ProfilePhoto = user.ProfilePhoto;
            _context.Users.Update(dbUser);
            await _context.SaveChangesAsync();

            return dbUser;
        }

        public async Task<Boolean> DeleteUser(int id)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser == null)
                return false;
            
            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}