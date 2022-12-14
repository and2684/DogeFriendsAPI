using System.Security.Cryptography;
using AutoMapper.QueryableExtensions;
using DogeFriendsAPI.Dto;

namespace DogeFriendsAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILoggerManager _logger;

        public UserRepository(DataContext context, IMapper mapper, ITokenService tokenService, ILoggerManager logger)
        {
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<List<UserShowDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            _logger.LogInfo($"Показываем пользователей. В списке {users.Count()} пользователей.");
            return _mapper.Map<List<User>, List<UserShowDto>>(users);
        }

        public async Task<UserShowDto?> GetUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return (user == null) ? null : _mapper.Map<User, UserShowDto>(user);
        }

        public async Task<List<UserShowDto>> GetUsersAsync(string username)
        {
            var users = await _context.Users.Where(x => x.Username.ToLower().Contains(username.ToLower())).ToListAsync();
            return _mapper.Map<List<User>, List<UserShowDto>>(users);
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

        public async Task<UserDto> RegisterUser(RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();

            var user = new User();

            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            user.FirstName = registerDto.FirstName;
            user.LastName = registerDto.LastName;
            user.IsCompany = registerDto.IsCompany;

            user.Roles = await _context.Roles.Where(x => x.Role.ToLower() == "user").ToListAsync();

            _context.Users!.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsCompany = user.IsCompany,
                Roles = user.Roles.Select(x => x.Role).ToList()
            };
        }

        public async Task<UserDto> LoginUser(LoginDto loginDto)
        {
            var user = await _context.Users.Where(x => x.Username == loginDto.Username.ToLower())
                .Include(x => x.Roles)
                .FirstOrDefaultAsync();

            return new UserDto
            {
                Username = user!.Username,
                Token = _tokenService.CreateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsCompany = user.IsCompany,
                Roles = user.Roles?.Select(x => x.Role).ToList()
            };
        }

        public async Task<bool> UserExist(string username)
        {
            return await _context.Users.Where(x => x.Username.ToLower() == username.ToLower()).AnyAsync(); // if user found return true
        }
        public async Task<bool> PasswordCorrect(LoginDto loginDto)
        {
            var user = await _context.Users.Where(x => x.Username == loginDto.Username.ToLower()).FirstOrDefaultAsync();

            using var hmac = new HMACSHA512(user!.PasswordSalt!);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user!.PasswordHash![i]) return false;
            }

            return true;
        }

        public async Task<bool> SetUserRoles(UserRoleSetDto userRoleSetDto)
        {
            var user = await _context.Users
                .Include(x => x.Roles)
                .Where(x => x.Username.ToLower() == userRoleSetDto.Username.ToLower())
                .FirstOrDefaultAsync();

            user!.Roles!.RemoveAll(x => true); // Чистим список ролей Юзера

            // И заполняем новым списком
            foreach(var role in userRoleSetDto.UserRoles!)
            {
                var userRole = await _context.Roles.Where(x => x.Role.ToLower() == role.ToLower()).FirstOrDefaultAsync();
                if (userRole != null)
                    user!.Roles.Add(userRole);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExist(int userId)
        {
            return await _context.Users.FindAsync(userId) != null; // if user found return true
        }
    }
}