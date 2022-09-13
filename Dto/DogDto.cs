namespace DogeFriendsAPI.Dto
{
    public class DogDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BreedId { get; set; }
        public string BreedName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public byte[]? MainPhoto { get; set; }
    }
}