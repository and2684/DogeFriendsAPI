namespace DogeFriendsAPI.Models
{
    public class DogPhotoLike
    {
        public User? SourceUser { get; set; }
        [Required]        
        public int SourceUserId { get; set; }
        public DogPhoto? LikedPhoto { get; set; }
        [Required]        
        public int LikedPhotoId { get; set; }        
    }
}