using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Constants;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.Reader.Data.Entities;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.Reader.Infrastructure.Configurations
{
    public class BookReviewConfiguration: EfCoreEntityConfiguration<BookReview>
    {
        public override void Configure(EntityTypeBuilder<BookReview> builder)
        {
            base.Configure(builder);
            builder.ToTable("BookReviews", schema: "reader");
            builder.Property(x => x.Content).IsRequired().IsUnicode().HasMaxLength(SharedValidationRules.TextAreaMaxLength);
        }
    }
}