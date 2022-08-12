namespace DogeFriendsAPI.Dto
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsCompany { get; set; }
        public byte[]? ProfilePhoto { get; set; }        
    }
}