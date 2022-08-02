namespace DogeFriendsAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetUsersAsync(string username)
        {
            return await _context.Users.Where(x => x.Username.ToLower().Contains(username.ToLower())).ToListAsync();
        }        

        public async Task<User> InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            
            await SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(User user)
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

            await SaveChangesAsync();
            return dbUser;
        }

        public async Task<Boolean> DeleteUserAsync(int id)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser == null)
                return false;
            
            _context.Users.Remove(dbUser);           
            await SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}