using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoHome.Models.Entity;
using PhotoHome.Models.Configurations;

namespace UltraWebsite.Models.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public int Index { get; set; }
        public Default Default { get; set; }

        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            Index = 1;
            Default = new Default();

            foreach (var item in Default.GetTagNames())
            {
                builder.HasData(new Tag { Id = Index++, Name = item });
            }
        }
    }
}
