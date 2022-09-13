using DogeFriendsAPI.Dto;


namespace DogeFriendsAPI.Interfaces
{
    public interface IDogRepository
    {
        public Task<List<DogDto>> GetAllUserDogsAsync(int userId);
        public Task<Dog?> GetDogAsync(int id);
        public Task<DogDto> InsertDogAsync(DogDto dog);
        public Task<DogDto?> UpdateDogAsync(DogDto dog);
        public Task<Boolean> DeleteDogAsync(int id);
        public Task<Boolean> DogExist(int userId, string dogName);
        public Task SaveChangesAsync();        
    }
}