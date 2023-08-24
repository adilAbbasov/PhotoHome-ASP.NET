namespace PhotoHome.Models.Entity
{
	public class Category : Entity
	{
		public Category() { }

		public string Name { get; set; }
		public string? ImageUrl { get; set; }
		public virtual ICollection<Picture> Images { get; set; }
	}
}
