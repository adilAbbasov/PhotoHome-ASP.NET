﻿using PhotoHome.Models.Entity;

namespace PhotoHome.Models.ViewModels
{
	public class AddImageViewModel
	{
		public string Description { get; set; }
		public string Title { get; set; }
		public IFormFile ImageUrl { get; set; }

		public int LikeCount { get; set; }
		public int DownloadCount { get; set; }

		public int userId { get; set; }
		public int categoryId { get; set; }
		public string[] Tags { get; set; }
	}
}
