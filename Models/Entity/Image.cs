namespace PhotoHome.Models.Entity
{
	public class Picture : Entity
	{
		public Picture() { }

		public string ImageUrl { get; set; }
		public int LikeCount { get; set; }
		public int DownloadCount { get; set; }

		public string Description { get; set; }
		public string Title { get; set; }

		public virtual User User { get; set; }
		public string? UserId { get; set; }

		public bool? Allow { get; set; }
		public virtual ICollection<ImageLike> ImageLikes { get; set; }
		public virtual ICollection<ImageTag> ImageTags { get; set; }

		public virtual Category Category { get; set; }
		public int? CategoryId { get; set; }
	}
}
