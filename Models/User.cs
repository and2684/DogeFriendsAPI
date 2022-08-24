using Microsoft.AspNetCore.Identity;

namespace DogeFriendsAPI.Data
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsCompany { get; set; }
        public byte[]? ProfilePhoto { get; set; }
    }
}
