using AutoMapper.QueryableExtensions;
using DogeFriendsAPI.Dto;

namespace DogeFriendsAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return await _context.Users.Where(x => x.Username.ToLowerInvariant().Contains(username.ToLowerInvariant())).ToListAsync();
        }

        public async Task<List<PersonDto>> GetPersonsAsync(string fullname)
        {
            var personList = await _context.Users.ProjectTo<PersonDto>(_mapper.ConfigurationProvider).ToListAsync();
            return personList.Where(x => x.FullName.ToLowerInvariant().Contains(fullname.ToLowerInvariant())).ToList();
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