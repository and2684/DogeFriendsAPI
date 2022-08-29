namespace DogeFriendsAPI.Models
{
    [Index(nameof(Role), IsUnique = true)]
    public class UserRole
    {
        public int Id { get; set; }
        [Required]
        public string Role { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new List<User>();
    }
}