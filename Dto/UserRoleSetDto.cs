namespace DogeFriendsAPI.Dto
{
    public class UserRoleSetDto
    {
        public int UserId { get; set; }
        public List<int>? UserRoles { get; set; }
    }
}