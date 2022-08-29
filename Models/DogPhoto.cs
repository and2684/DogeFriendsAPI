namespace DogeFriendsAPI.Models
{
    public class DogPhoto
    {
        public int Id { get; set; }
        [Required]
        public byte[]? Photo { get; set; }
        public bool IsMain { get; set; }
        [Required]
        public int DogId { get; set; }
        public Dog? Dog { get; set; }        
        public List<DogPhotoLike>? LikedByUsers { get; set; }
    }
}