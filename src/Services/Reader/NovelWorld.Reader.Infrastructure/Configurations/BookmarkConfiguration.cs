using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Constants;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.Reader.Data.Entities;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.Reader.Infrastructure.Configurations
{
    public class BookmarkConfiguration: EfCoreEntityConfiguration<Bookmark>
    {
        public override void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            base.Configure(builder);
            builder.ToTable("Bookmarks", schema: "reader");
            builder.Property(x => x.Note).IsRequired().IsUnicode().HasMaxLength(SharedValidationRules.TextAreaMaxLength);
        }
    }
}