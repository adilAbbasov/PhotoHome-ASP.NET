using Microsoft.AspNetCore.Identity;

namespace PhotoHome.Models.Entity
{
    public class User : IdentityUser
    {
        public User() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Message { get; set; }
        public string? About { get; set; }

        public string? ImageUrl { get; set; }
        public virtual ICollection<Picture> CreatedImages { get; set; }
        public virtual ICollection<ImageLike> LikedImages { get; set; }
    }
}
