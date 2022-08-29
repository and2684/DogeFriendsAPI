namespace DogeFriendsAPI.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int BreedId { get; set; }
        public Breed? Breed { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<DogPhoto>? Photos { get; set; }
    }
}