using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogeFriendsAPI.Models
{
    public class Breed
    {
        public int Id { get; set; }
        [Required]
        public string BreedName { get; set; } = string.Empty;
        public List<Dog> Dogs {get; set;} = new List<Dog>();
    }
}