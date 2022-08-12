namespace DogeFriendsAPI.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = String.Empty;
        [Required]
        public string Password { get; set; } = String.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsCompany { get; set; }        
    }
}