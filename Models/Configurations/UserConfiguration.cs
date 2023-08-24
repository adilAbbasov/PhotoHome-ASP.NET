using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhotoHome.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PhotoHome.Data;

namespace PhotoHome.Models.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public int Index { get; set; }

		public void Configure(EntityTypeBuilder<User> builder)
		{
			Index = 1;

			builder.HasData(new User { Id = Index++.ToString(), UserName = "Hesen_Rzayev", FirstName = "Hesen", Email = "hsnrz2002@gmail.com", LastName = "Rzayev", ImageUrl = "~\\images\\user\\adilabbasov.png" });
			builder.HasData(new User { Id = Index++.ToString(), UserName = "Adil_Abbasov", FirstName = "Adil", Email = "ffff@gmail.com", LastName = "Abbasov" });
		}
	}
}
