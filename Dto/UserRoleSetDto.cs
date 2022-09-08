namespace DogeFriendsAPI.Dto
{
    public class UserRoleSetDto
    {
        public string Username { get; set; } = string.Empty;
        public List<string>? UserRoles { get; set; }
    }
}