using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.Design;
using System.Net;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System;
using System.Threading;
using System.Threading.Tasks;
using PhotoHome.Models.Entity;
using CloudinaryDotNet.Actions;
using PhotoHome.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace UltraWebsite.Models.Configurations
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public int Index { get; set; }

		public void Configure(EntityTypeBuilder<Category> builder)
		{
			Index = 1;

			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/bustling-city-street-in-india.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Around The World" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/office-flat-lay-on-wooden-desk-with-catch-tray.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Work From Home" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/a-note-just-to-say-you-are-enough.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Mental Health" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/autumn-photographer-taking-picture.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Female Photographer" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/large-fall-leaf-in-hand.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Fall" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/creamy-cold-drink-sits-on-a-wooden-table.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Coffee" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/woman-resting-her-feet-by-the-window.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Home" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/brushes-blossoms.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Beauty" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/ready-set-snow.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Fitness" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/a-minimal-yet-cosy-workspace.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Office" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/kids-show-mom-some-love.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Kids" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/looking-back-through-arches-in-india.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "India" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/two-tone-ink-cloud.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Wallpapers" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/a-tattooed-hand-doing-the-sign-for-i-love-you.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Sign Language" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/model-with-leather-jacket-over-shoulders.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Womens Fashion" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/black-and-white-crosswalk.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Black And White" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/mixing-board-black-and-white.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Technology" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/person-in-a-white-shirt-sits-in-front-of-a-brick-wall.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Mens Fashion" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/ice-cracks-on-a-frozen-sea.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Textures" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/a-drop-of-pink-and-yellow-paint-in-water.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Background" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/team-working-together-with-laptops.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Business" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/blue-lake-and-rocky-mountains.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Nature" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/ripe-red-strawberries-in-a-white-bowl.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Food" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/man-holding-shipping-box-on-red-brick.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Product" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/person-stands-by-a-brick-wall-and-holds-a-book-open.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Jewellry" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/triangle-goat-collar.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Animal" });
			builder.HasData(new Category { Id = Index++, ImageUrl = "https://burst.shopifycdn.com/photos/incredible-balance-yoga-posing.jpg?width=1200&amp;format=pjpg&amp;exif=0&amp;iptc=0", Name = "Yoga" });

		}
	}
}
