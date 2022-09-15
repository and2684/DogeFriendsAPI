using DogeFriendsAPI.Dto;

namespace DogeFriendsAPI.Data
{
    public class DogRepository : IDogRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DogRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> DeleteDogAsync(int id)
        {
            var dbDog = _context.Dogs.FindAsync(id).Result;
            if (dbDog == null)
                return false;

            _context.Remove(dbDog);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> DogExist(int userId, string dogName)
        {
            var dbUser = await _context.Users.FindAsync(userId);
            var dbUserdogs = await _context.Dogs.Where(x => x.User == dbUser).ToListAsync();
            var dbDog = dbUserdogs.Where(x => x.Name == dogName).FirstOrDefault();

            return dbDog != null;
        }

        public async Task<List<DogDto>> GetAllUserDogsAsync(int userId)
        {
            var dbUser = await _context.Users.FindAsync(userId);
            var dbUserdogs = await _context.Dogs
                .Include(d => d.Breed)
                .Where(x => x.User == dbUser)
                .ToListAsync();

            return _mapper.Map<List<Dog>, List<DogDto>>(dbUserdogs);
        }

        public async Task<DogDto?> GetDogAsync(int id)
        {
            var doge = await _context.Dogs
                .Include(b => b.Breed)
                .Include(u => u.User)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<Dog, DogDto>(doge!);
        }

        public async Task<DogDto> InsertDogAsync(DogDto dog)
        {
            var dbDog = new Dog();
            dbDog.Name = dog.Name;
            dbDog.BreedId = dog.BreedId;
            dbDog.Breed = await _context.Breeds.FindAsync(dog.BreedId);
            dbDog.UserId = dog.UserId;
            dbDog.User = await _context.Users.FindAsync(dog.UserId);

            _context.Add(dbDog);
            await SaveChangesAsync();

            return dog;
        }

        public async Task<DogDto?> UpdateDogAsync(DogDto dog)
        {
            var dbDog = await _context.Dogs.FirstOrDefaultAsync(x => x.Id == dog.Id);
            if (dbDog == null) return null;

            dbDog.Name = dog.Name;
            dbDog.BreedId = dog.BreedId;
            dbDog.Breed = await _context.Breeds.FindAsync(dog.BreedId);
            dbDog.UserId = dog.UserId;
            dbDog.User = await _context.Users.FindAsync(dog.UserId);

            _context.Update(dbDog);
            await SaveChangesAsync();

            return _mapper.Map<Dog, DogDto>(dbDog);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}