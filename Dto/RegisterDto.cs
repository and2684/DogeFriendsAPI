namespace DogeFriendsAPI.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = String.Empty;
        [Required][MinLength(4)]
        public string Password { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public bool IsCompany { get; set; }        
    }
}