namespace DogeFriendsAPI.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondaryName { get; set; } = string.Empty;
        public bool IsCompany { get; set; }
        public byte[]? ProfilePhoto { get; set; }
    }
}
